function eventBinding() {
    $('#factory-selecter').on('click', '.facroty-btn', function () {

        console.log($(this).data('id'));

        var clickFactoryID = $(this).data('id');


        var postData = {
            'id': clickFactoryID
        };

        ajaxCall('getFactoryInfoById', postData , function (error, result) {

            if (!error) {
                updateFactoryInfo($.parseJSON(result));
            } else {
                alert('get factory error!');
            }

        });


    });
}

function updateCompanyInfo() {

    $('#company-img').attr('src', _COMPANYObj.LogoURL);
    $('#company-name').text(_COMPANYObj.Name);
    $('#company-short-name').text(_COMPANYObj.ShortName);
    $('#company-email').text(_COMPANYObj.ContactEmail);
    $('#company-culture').text(_COMPANYObj.CultureInfoName);
    $('#company-address').text(_COMPANYObj.Address);
}

function updateFactoryButton() {

    var createHTML = '';

    for (var i = 0; i < _FactoryList.length; i++) {
        createHTML = createHTML + '<button class="facroty-btn btn btn-primary" data-id="' + _FactoryList[i].Id +'">' + _FactoryList[i].Name + '</button>';
    }
    $('#factory-selecter').append(createHTML);
}

function updateFactoryInfo(factoryInfo) {

    console.log(factoryInfo);

    $('#select-hint').fadeOut();

    $('#factory-img').attr('src', factoryInfo.PhotoURL);
    $('#factory-name').text(factoryInfo.Name);
    $('#factory-latitude').text(factoryInfo.Latitude);
    $('#factory-longitude').text(factoryInfo.Longitude);
    $('#factory-description').text(factoryInfo.Description);
    $('#factory-timeZone').text(factoryInfo.TimeZone);

    $('#factory-info-true').fadeIn();

}


$(document).ready(function () {

    updateCompanyInfo();
    updateFactoryButton();
    eventBinding();
});