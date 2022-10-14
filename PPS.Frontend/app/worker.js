onmessage = function (params) {
    var xhr = new XMLHttpRequest;
    xhr.open('GET', params.data.serviceBase + 'Token')
    xhr.onreadystatechange = function (response) {
        if (xhr.readyState === 4 && xhr.status === 200) {
            postMessage(response);
        }
    }
    xhr.send();
    //postMessage(true);
    return;
};
