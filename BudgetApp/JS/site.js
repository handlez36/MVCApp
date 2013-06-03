
/// <reference path="../Scripts/jquery-1.5.1-vsdoc.js" />
/// <reference path="../Scripts/jquery-ui-1.8.11.js" />
/// <reference path="../Scripts/jquery.jeditable.js" />
/// <reference path="../Scripts/accounting.js" />


var creditcardentry = {};

var Money = function (amount) {
    this.amount = amount;
}

Money.prototype.valueOf = function () {
    return parseFloat(Math.round(this.amount * 100) / 100).toFixed(2);
}

$(document).ready(function () {
    var entry_clicked_id = "Nothing";

    creditcardentry.expandform();

    $("[id^=entry-date]").datepicker({
        onSelect: function (selectedDate) {
            var request = $.get("CreditCard/UpdateField",
                                { id: $(this).attr("id"), value: selectedDate });
        }
    });

    $('#date-input-field').datepicker();

    $("[id^=scheduledate]").datepicker({
        onSelect: function (selectedDate) {
            var request = $.get("CreditCard/UpdateField",
                                { id: $(this).attr("id"), value: selectedDate },
                                function (data) {
                                    $.get("CreditCard/UpdatePaymentPlans",
                                    {}, function (data) {
                                        $('#payment-table').html(data);
                                    });
                                }
                                );
        }
    });

    /**
    * Grabs EntryID after a cell with class '.creditlistcell' has been clicked
    * @param
    * @return
    */
    $('.creditlist-editablecell').click(function () {
        var entry_clicked = $(this).attr("id");
        var string_segments = entry_clicked.split('-')
        entry_clicked_id = string_segments[2];
    });

    $('.edit-spinner').change(function (e) {
        $.get("CreditCard/UpdateField",
                { id: $(this).attr("id"), value: $(this).val() },
                function (data) {
                    $.get("CreditCard/UpdatePaymentPlans",
                            {}, function (data) {
                                $('#payment-table').html(data);
                          });
                }
              );
    });


    /** 
    * Perform backend processing of cell updates for any cell
    * @param
    * @return
    */
    $('.creditlist-editablecell').editable("CreditCard/UpdateField",
    {
        submitdata: {
            EntryId: function () {
                return entry_clicked_id;
            }
        },
        callback: function (value, settings) {


            // Calculate remaining balance after price change
            var tmp_edit_amount = $('#entry-amount-' + entry_clicked_id).html();
            var edit_amount = new Money(parseFloat(tmp_edit_amount).toFixed(2));

            var amount_paid =
                new Money(
                    parseFloat($('#amount-paid-' + entry_clicked_id).html())
                 )

            var remaining_balance = accounting.formatMoney(
                eval(edit_amount - amount_paid)
            );
            $('#amount-remaining-' + entry_clicked_id).html("" + remaining_balance);


            // Update Payments List
            var request = $.get("CreditCard/UpdatePaymentPlans", {},
                                   function (data) {
                                       $('#payment-table').html(data);
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