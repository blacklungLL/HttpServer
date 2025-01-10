document.addEventListener('DOMContentLoaded', function () {
    const loginButton = document.getElementById('authButton');

    loginButton.addEventListener('click', function () {
        console.log('Login button clicked');
        // Перенаправляем на страницу auth/login
        window.location.href = 'auth/login';
    });
});