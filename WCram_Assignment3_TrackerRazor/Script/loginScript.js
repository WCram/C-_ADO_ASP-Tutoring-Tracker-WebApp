
// Validation for login screen
function formValidate() {
    var studentId = document.getElementById("studentId").value;
    var password = document.getElementById("password").value;
    var validate = true;
    var idRegEx = /^\d{9,10}$/;

    //Checks if the input is empty

    if (password === "") {
        document.getElementById("idError").innerHTML = " * Enter a valid student number.";

        validate = false;   
    }
    else {
        document.getElementById("idError").innerHTML = "";
    }

    // Checks if password input is empty. Password check not currently implemented
    if (password === "") {
        document.getElementById("passwordError").innerHTML = " * Enter a password.";
        validate = false;
    }
    else {
        document.getElementById("passwordError").innerHTML = "";
    }

    // If information is validated sends the userName to the login page.
    // Will be altered in the future when real database is integrated
    if (validate) {
        document.getElementById("usersName").value = userName(studentId);
    }


    return validate;
}

