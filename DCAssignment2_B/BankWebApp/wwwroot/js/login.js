function loadLogin(status) {
    var url = '/api/login';

    if (status === 'authed') {
        url = '/api/login/authed';
    }

    if (status === 'error') {
        url = '/api/login/error';
    }

    fetch(url)
        .then(response => {
            if (!response.ok) {
                console.log('Response ! ok');
                throw new Error('Response was not ok :(');
            }
            return response.text();
        })
        .then(data => {
            console.log('Data Retrieved: ' + data);
            document.getElementById('login-body').innerHTML = data;
        })
        .catch(error => {
            console.error('Fetch error:', error);
        });
}

function authenticate() {
    var email = document.getElementById('AuthEmail').value;
    var password = document.getElementById('AuthPW').value;

    var user = {
        Email: email,
        Password: password
    }

    console.log(user);

    const headers = {
        'Content-Type': 'application/json'
    };

    const requestOptions = {
        method: 'POST',
        headers: headers,
        body: JSON.stringify(user)
    }

    const url = '/api/login/authenticate';

    //Call authenticate API
    fetch(url, requestOptions).then(res => {
        if (!res.ok) {
            throw new Error('Response was not ok');
        }
        return res.json();
    }).then(data => {
        const jsonObj = data;
        if (jsonObj.login) {
            loadLogin('authed');
        } else {
            loadLogin('error');
        }
    })

    console.log('Authenticate Called');
}