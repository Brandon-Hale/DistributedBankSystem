function loadTransaction(status) {
    var url = '/api/transaction';
    if (status === 'new') {
        url = '/api/transaction/new';
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
            document.getElementById('transaction-body').innerHTML = data;
        })
        .catch(error => {
            console.error('Fetch error:', error);
        });
}

function search() {
    var input = document.getElementById('searchBar');
    var filter = input.value.toUpperCase();
    var list = document.getElementById('transaction-list');
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

function checkAdmin(user) {
    console.log('Check Admin Ran: ' + user);

    if (user === 'admin') {
        var newTransTab = document.getElementById('new-trans-tab');
        newTransTab.parentNode.removeChild(newTransTab);
    }
}

function performTransaction() {
    var transactionId = document.getElementById('transactionId').value;
    var accountNumber = document.getElementById('accountNumber').value;
    var toAccountNumber = document.getElementById('toAccountNumber').value;
    var balance = document.getElementById('balance').value;

    var transactionData = {
        TransactionId: transactionId,
        FromAccount: accountNumber,
        ToAccount: toAccountNumber,
        Amount: parseFloat(balance) // Parse the balance as a floating-point number
    };

    console.log(transactionData);

    const headers = {
        'Content-Type': 'application/json'
    };

    const requestOptions = {
        method: 'POST',
        headers: headers,
        body: JSON.stringify(transactionData)
    }

    const url = '/api/transaction/posttrans';

    fetch(url, requestOptions).then(res => {
        if (!res.ok) {
            throw new Error('Response was not ok');
        }
        return res.json();
    }).then(data => {
        const jsonObj = data;
        if (jsonObj.transaction) {
            loadTransaction('');
        }
    })
    console.log('Authenticate Called');

}