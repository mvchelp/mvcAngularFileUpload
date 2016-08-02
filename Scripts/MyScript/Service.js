myapp.service('studentservice', function ($http) {

    this.get = function () {
        var request = $http({
            method: 'GET',
            url: 'api/Students/GetStudents'
        });
        return request;
    }

    this.studentById = function (id) {
        var request = $http({
            method: 'GET',
            url: 'api/Students/GetStudentById',
            params: {
                id: id
            }
        });
        return request;
    }

    this.Save = function (file, student) {
        var formData = new FormData();
        formData.append("file", file);
        formData.append("FirstName", student.FirstName);
        formData.append("LastName", student.LastName);
        formData.append("Gender", student.Gender);
        formData.append("BirthDate", student.BirthDate);
        formData.append("EnrollmentNo", student.EnrollmentNo);
        formData.append("EmailId", student.EmailId);
        var request = $http({
            method: 'POST',
            url: 'api/Students/PostStudent',
            headers: { 'Content-Type': undefined },
            withCredentials: true,
            transformRequest: angular.identity,
            data: formData
        });
        return request;
    }

    this.update = function (file, student) {
        var formData = new FormData();
        formData.append("file", file);
        formData.append("StudentId", student.StudentId)
        formData.append("FirstName", student.FirstName);
        formData.append("LastName", student.LastName);
        formData.append("Gender", student.Gender);
        formData.append("BirthDate", student.BirthDate);
        formData.append("EnrollmentNo", student.EnrollmentNo);
        formData.append("EmailId", student.EmailId);
        var request = $http({
            method: 'PUT',
            url: 'api/Students/PutStudent',
            headers: { 'Content-Type': undefined },
            withCredentials: true,
            transformRequest: angular.identity,
            data: formData
        });
        return request;
    }
});