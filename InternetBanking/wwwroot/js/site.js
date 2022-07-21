document.addEventListener('DOMContentLoaded', () => {
    const userTypeFormField = document.getElementById('user-type-form-field');
    const amountFormField = document.getElementById('amount-form-field');

    if (userTypeFormField != null) {
        userTypeFormField.onclick = () => {
            const userTypeSelected = userTypeFormField.options[userTypeFormField.selectedIndex].text;

            if (userTypeSelected == 'Client') {
                amountFormField.hidden = false;
            } else {
                amountFormField.hidden = true;
            }
        }
    }
});