const limitInput = document.getElementById('limit-input');
const amountInput = document.getElementById('amount-input');
const productType = document.getElementById('product-type-select');

if (productType != null) {
    productType.onclick = () => {
        const productTypeSelected = productType.options[productType.selectedIndex].value;
        if (productTypeSelected == 3) {
            limitInput.hidden = false;
            amountInput.hidden = true;
        } else {
            limitInput.hidden = true;
            amountInput.hidden = false;
            console.log(amountInput)
        }
    }
}