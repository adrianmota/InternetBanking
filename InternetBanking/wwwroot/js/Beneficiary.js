let accountId;

const deleteButton = document.querySelector("#deleteButton")

const setBeneficiary = id => accountId = id

const deleteBeneficiary = () => {
    const http = window.location.protocol;
    const domain = window.location.host;
    const url = `${http}//${domain}/Home/DeleteBeneficiary?accountId=${accountId}`;

    window.location = url;
}

deleteButton.addEventListener("click", deleteBeneficiary);