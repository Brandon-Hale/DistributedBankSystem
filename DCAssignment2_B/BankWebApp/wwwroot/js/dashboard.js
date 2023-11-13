function loadDashboard(type) {
    console.log('loadDashboard Called: ' + type);

    var url = '/api/dashboard/';

    if (type === 'edit') {
        url = '/api/dashboard/edit';
    }
    if (type === 'users') {
        url = '/api/dashboard/users';
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
            document.getElementById('dashboard-body').innerHTML = data;
        })
        .catch(error => {
            console.error('Fetch error:', error);
        });
}

function editUser(userId) {
    var url = '/api/dashboard/edituser/' + userId;

    fetch(url).then(res => {
        if (!res.ok) {
            throw new Error('Response was not ok');
        }
        return res.text();
    }).then(data => {
        document.getElementById('dashboard-body').innerHTML = data;
    })
    .catch(error => {
        console.error('Fetch error:', error);
    });
}

function deleteUser(userId) {
    var url = '/api/dashboard/deleteuser/' + userId;

    const headers = {
        'Content-Type': 'application/json'
    };

    const requestOptions = {
        method: 'POST',
        headers: headers,
    }

    fetch(url, requestOptions).then(res => {
        if (!res.ok) {
            throw new Error('Response was not ok');
        }
    }).then(() => {
        loadDashboard('users');
    });
}

function userTransactions(userId) {

    var url = '/api/transaction/usertransactions/' + userId;

    fetch(url).then(res => {
        if (!res.ok) {
            throw new Error('Response was not ok');
        }
        return res.text();
    }).then(data => {
        document.getElementById('dashboard-body').innerHTML = data;
    })
    .catch(error => {
        console.error('Fetch error:', error);
    });
}

function performUpdate() {
    var userId = document.getElementById('userId').textContent;
    var name = document.getElementById('name').value;
    var email = document.getElementById('email').value;
    var phoneNumber = document.getElementById('phone').value;
    var password = document.getElementById('password').value;

    var userData = {
        UserId: userId,
        Name: name,
        Email: email,
        PhoneNo: phoneNumber,
        Password: password
    };

    console.log(userData);

    const headers = {
        'Content-Type': 'application/json'
    };

    const requestOptions = {
        method: 'PUT',
        headers: headers,
        body: JSON.stringify(userData)
    }

    const url = '/api/dashboard/updateuser';

    fetch(url, requestOptions).then(res => {
        if (!res.ok) {
            throw new Error('Response was not ok');
        }
        return res.json();
    }).then(data => {
        const jsonObj = data;
        if (jsonObj.user) {
            loadDashboard('');
        }
    })
    console.log('Authenticate Called');
}

function search() {
    var input = document.getElementById('searchBar');
    var filter = input.value.toUpperCase();
    var list = document.getElementById('users-list');
    var items = list.getElementsByTagName('li');

    for (var i = 0; i < items.length; i++) {
        var searchThrough = items[i].getElementsByTagName("class")
        var search = "";

        for (var ii = 0; ii < searchThrough.length; ii++) {
            search += searchThrough[ii].textContent;
        }
        console.log(search);

        if (search.toUpperCase().indexOf(filter) > -1) {
            items[i].style.display = "";
        } else {
            items[i].style.display = "none";
        }
    }
}

function redirectToTransactionHistory(accountNumber) {
    // Redirect to the transaction history page with the account number as a query parameter
    window.location.href = 'Home/Transaction#';
}
