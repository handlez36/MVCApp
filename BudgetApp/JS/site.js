
/// <reference path="../Scripts/jquery-1.5.1-vsdoc.js" />
/// <reference path="../Scripts/jquery-ui-1.8.11.js" />
/// <reference path="../Scripts/jquery.jeditable.js" />


var creditcardentry = {};

$(document).ready(function () {
    var entry_clicked_id = "Nothing";

    creditcardentry.expandform();

    $('#entry-date').datepicker();

    $("[id^=scheduledate]").datepicker();

    /**
    * Grabs EntryID after a cell with class '.creditlistcell' has been clicked
    * @param
    * @return
    */
    $('.creditlistcell').click(function () {
        var entry_clicked = $(this).attr("id");
        var string_segments = entry_clicked.split('-')
        entry_clicked_id = string_segments[2];
    });


    /** 
    * Perform backend processing of cell updates for any cell
    * @param
    * @return
    */
    $('.creditlistcell').editable("CreditCard/UpdateField",
    {
        submitdata: {
            EntryId: function () {
                return entry_clicked_id;
            }
        },
        callback: function (value, settings) {
            // Calculate remaining balance after price change
            var edit_amount = parseFloat($('#entry-amount-' + entry_clicked_id).html());
            var amount_paid = parseFloat($('#amount-paid-' + entry_clicked_id).html());
            var remaining_balance = eval(edit_amount - amount_paid);
            $('#amount-remaining-' + entry_clicked_id).html("" + remaining_balance);

            // Update Payments List
            var request = $.get("CreditCard/ListPayments", function (data) {
                alert(data);
            });

        }
    });
});


/** 
    * Expand or collapse 'Add New' form when the appropriate links
        or buttons are clicked
    * @param
    * @return
*/
creditcardentry.expandform = function () {
    $("#addentry").click(function (e) {
        $("#editform").fadeIn('slow');

        e.PreventDefault();
        return false;
    })

    $("#canceladd").click(function(e) {
        $("#editform").fadeOut('fast');

        e.PreventDefault();
        return false;
    })
};