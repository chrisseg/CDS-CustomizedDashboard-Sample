//Create Form Data
function createPostData(postObj) {

    var postData = new FormData();

    $.each(postObj, function (key, value) {
        postData.append(key, value);
    });

    return postData;

}

//Ajax calling CDS-API
function ajaxCall(actionName, parameterObj, callback) {

    var endPoint = "/Dashboard/ReqAction?action=" + actionName;

    if (parameterObj && parameterObj.id) {
        endPoint = endPoint + "&Id=" + parameterObj.id;
    }

    var postData = null;

    if (parameterObj.postData) {
        postData = createPostData(parameterObj.postData);
    }

    xhr = $.ajax({
        type: "POST",
        url: endPoint + "&t=" + Date.now(),
        data: postData,
        cache: false,
        contentType: false,
        processData: false,
        success: function (data) {

            if (callback) {
                callback(null, data);
            }


        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {

            if (callback) {
                callback(true, null);
            }

            if (XMLHttpRequest.status === 401) {
                toastr["error"]("[[[Session Expired. Please Re-Login]]].");
                // setTimeout(function () { sfBacktoHomeIndex(); }, 2000);
            }
        }
    });


};