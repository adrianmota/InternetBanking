
    console.log("ibhuikjn")
    let benId;

    const deleteButton = document.querySelector("#deleteButton")

    const setBeneficiary = (id) => {
        benId = id;
        console.log(benId);
    }

    const deleteBeneficiary = () => {
        const http = window.location.protocol;
        const domain = window.location.host;
        const url = `${http}//${domain}/Home/DeleteBeneficiary?accountId=${benId}`;

        console.log(url)
        window.location = url;
    }

    deleteButton.addEventListener("click", deleteBeneficiary );