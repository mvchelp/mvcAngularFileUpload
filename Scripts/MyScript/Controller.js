myapp.controller('studentcontroller', function ($scope, studentservice) {

    initControl();
    function initControl()
    {
        loadStudents();
        $scope.DetailView = true;
        $scope.AddView = false;
        $scope.EditView = false;
        $scope.EditImageView = false;
    }

    function loadStudents() {
        $scope.Students = studentservice.get()
            .success(function (response) {
                $scope.students = response;
            })
    }

    $scope.GetStudentById = function (student)
    {
        studentservice.studentById(student.StudentId)
            .success(function (response) {
                $scope.Student = response;
            })
    }

    $scope.Add = function ()
    {
        $scope.FileInvalidMessage = null;
        $scope.DetailView = false;
        $scope.AddView = true;
        $scope.studentForm.$setPristine();
    }

    $scope.Edit = function (student) {
        $scope.FileInvalidMessage = null;
        $scope.studentForm.$setPristine();
        $scope.DetailView = false;
        $scope.AddView = false;
        $scope.GetStudentById(student);
        $scope.EditView = true;
        $scope.ShowImage = true;
    }

    $scope.EditImage = function ()
    {
        $scope.EditImageView = true;
        $scope.ShowImage = false;
    }

    $scope.emailpattern = /^[a-z]+[a-z0-9._]+@[a-z]+\.[a-z.]{2,5}$/;

    $scope.ImageValidate = function (file) {
        $scope.FileInvalidMessage = null;
        if (file != null || file == "") {
            $scope.SelectedFile = file[0];
            $scope.filename = file[0].name;
            var filetype = file[0].type;
            var filesize = file[0].size;
            var Extension = filetype.split("/");
            Extension = Extension[Extension.length - 1].toLowerCase();
            if (Extension == "jpg" || Extension == "jpeg" || Extension == "png" || Extension == "gif") {
                if (filesize <= 2097152) {
                    return $scope.filename;
                } else {
                    $scope.FileInvalidMessage = "Maximum Image Size Allowed is 2MB"
                }
            }
            else {
                $scope.FileInvalidMessage = "Please Select Image File(jpg, jpeg, png, gif)";
            }
        } else {
            $scope.FileInvalidMessage = "Please Select Image....!";
        }
    }

    $scope.Save = function () {
        if ($scope.studentForm.$invalid) {
            if ($("#file").val() == null || $("#file").val() == "") {
                $scope.FileInvalidMessage = "Please Select Image....!";
            }
            return;
        } else {
            if ($("#file").val() == null || $("#file").val() == "") {
                $scope.FileInvalidMessage = "Please Select Image....!";
                return;
            } else {
                studentservice.Save($scope.SelectedFile, $scope.Student)
                    .success(function (status) {
                        ClearControl();
                        initControl();
                    }).error(function (data, status, headers, config) {
                        alert("Error");
                    });
            }
        }
    }

    $scope.Update = function () {
        if ($scope.studentUpdateForm.$invalid) {
            if ($("#Editfile").val() == null || $("#Editfile").val() == "") {
                $scope.FileInvalidMessage = "Please Select Image....!";
            }
            return;
        } else {

            if ($scope.EditImageView == true) {
                if ($("#Editfile").val() == null || $("#Editfile").val() == "") {
                    $scope.FileInvalidMessage = "Please Select Image....!";
                    return;
                }
                studentservice.update($scope.SelectedFile, $scope.Student)
                    .success(function (status) {
                        ClearControl();
                        initControl();
                    }).error(function (data, status, headers, config) {
                        alert("Error");
                    });
            }
        }
    }

    function ClearControl()
    {
        $("#file").val('');
        $scope.Student = null;
        $scope.FileInvalidMessage = null;
        $scope.SelectedFile = null;
    }

    $scope.Cancel = function ()
    {
        ClearControl();
        initControl();
    }

});

