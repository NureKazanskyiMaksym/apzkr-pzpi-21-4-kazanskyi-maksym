/*function add_auth_header(url, options = {}) {

}

(function () {
    // Store the original fetch function
    const originalFetch = window.fetch;

    // Override the window.fetch function
    window.fetch = function (url, options = {}) {
        // Retrieve the stored token from localStorage
        console.log("lamda");
        const token = localStorage.getItem('token');
        if (token) {
            // Ensure the headers object exists in the options
            if (!options.headers) {
                options.headers = {};
            }
            // Append the Authorization header with the stored credentials
            options.headers['Authorization'] = `Basic ${token}`;
        }

        // Proceed with the original fetch request using the modified options
        return originalFetch(url, options);
    };
})();*/