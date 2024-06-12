function formatNumbersWithDashesKeyPress(event, textbox) {
    var charCode = event.which || event.keyCode;
    var char = String.fromCharCode(charCode);

    if (!/\d/.test(char)) {
        event.preventDefault();
        return false; // Allow only digits
    }

    event.preventDefault();
    var currentValue = textbox.value + char;

    // Remove all non-digit characters
    currentValue = currentValue.replace(/\D/g, '');

    // Format the string by inserting dashes every 4 digits
    var formattedID = '';
    if (currentValue.length <= 4) {
        formattedID = currentValue;
    } else if (currentValue.length <= 8) {
        formattedID = currentValue.substring(0, 4) + '-' + currentValue.substring(4);
    } else {
        formattedID = currentValue.substring(0, 4) + '-' + currentValue.substring(4, 8) + '-' + currentValue.substring(8);
    }


    // Set the formatted value back to the textbox
    textbox.value = formattedID;

}