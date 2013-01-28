
/// <reference path="../Scripts/jquery-1.5.1-vsdoc.js" />
/// <reference path="../Scripts/jquery-ui-1.8.11.js" />
/// <reference path="../Scripts/jquery.jeditable.js" />


var creditcardentry = {};

$(document).ready(function () {
    creditcardentry.expandform();

    $('#date-input-field').datepicker();

    $('.creditlistcell').editable("CreditCard/UpdateField",
    {
        submitdata : {
            EntryId: function () {
                return $("#EntryId").val();
            }
        }
    });
});

creditcardentry.expandform = function () {
    $("#addentry").click(function (e) {
        $("#editform").fadeIn('slow');

        e.PreventDefault();
        return false;
    })
};