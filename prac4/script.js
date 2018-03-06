
var uri = 'api/player';

$(document).ready(function () {
    $.ajax({
        type: 'GET',
        url: uri,
        success: function (data) {
            displayData(data);
        },
        error: function (jqXHR, textStatus, err) {
            $('#player').text('Error: ' + err);
        }
    })
});

function formatItem(item) {
    return item.Registration_ID + ', ' + item.Player_name + ', ' + item.Team_name + ', '
        + item.Date_of_birth.replace("T00:00:00", '');
}

function displayData(data) {
    $('ul').empty();
    $.each(data, function (key, item) {
        $('<li>', { text: formatItem(item) }).appendTo($('#players'));
    });
}

function searchData(data) {
    $.each(data, function (key, item) {
        var result = item.Player_name.toLowerCase().search($('#playerId').val().toLowerCase());
        if (result >= 0) {
            $.ajax({
                type: 'GET',
                url: uri + '/' + item.Registration_ID,
                success: function (data) {
                    $('#player').text(formatItem(data));
                },
                error: function (jqXHR, textStatus, err) {
                    $('#player').text('Error: ' + err);
                }
            })
        }
    })
}

function find() {
    if ($('#Select1 :selected').text() == "ID") {
        var id = $('#playerId').val();

        $.ajax({
            type: 'GET',
            url: uri + '/' + id,
            success: function (data) {
                $('#player').text(formatItem(data));
            },
            error: function (jqXHR, textStatus, err) {
                $('#player').text('Error: ' + err);
            }
        })
    } else {
        $.ajax({
            type: 'GET',
            url: uri,
            success: function (data) {
                searchData(data);
            },
            error: function (jqXHR, textStatus, err) {
                $('#player').text('Error: ' + err);
            }
        })
    }
}

function del() {
    var id = $('#playerId').val();

    $.ajax({
        type: 'DELETE',
        url: uri + '/' + id,
        success: function (data) {
            displayData(data);
        },
        error: function (jqXHR, textStatus, err) {
            $('#player').text('Error: ' + err);
        }
    })
}

function update() {
    var playerName;
    var dob = $('#dob').val().split('-');

    if ($('#fname').val() != "" && $('#lname').val() != "" && dob[0] != null && dob[1] != null && dob[2] != null) {
        if (!isNaN(dob[0]) && !isNaN(dob[1]) && !isNaN(dob[2])) {
            playerName = $('form').serialize().replace("&LName=", '+');
        } else {
            playerName = "Registration_ID=&Player_name=&Team_name=&Date_of_birth=";
        }
    } else {
        playerName = "Registration_ID=&Player_name=&Team_name=&Date_of_birth=";
    }
    $.ajax({
        type: 'POST',
        url: uri,
        data: playerName,
        success: function (data) {
            $('#error').text('');
            displayData(data);
        },
        error: function (jqXHR, textStatus, err) {
            $('#error').text('Error: Invalid Input');
        }
    })
}
