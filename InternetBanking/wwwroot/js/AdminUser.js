let userId;

const activeUser = document.querySelector("#btnActiveUser")
const deactiveUser = document.querySelector("#btnDeactiveUser")

const setUser = id => userId = id;

const setState = () => {
    const http = window.location.protocol;
    const domain = window.location.host;
    const url = `${http}//${domain}/User/SetUserStatus?id=${userId}`;

    window.location = url;
}

<<<<<<< HEAD
activeUser.addEventListener("click", setState);
deactiveUser.addEventListener("click", setState);
=======
    activeUser.addEventListener("click", () => { setState() });
    deactiveUser.addEventListener("click", () => { setState() });
});
>>>>>>> 96b93cca98d67a35c6d912c2feee47c6f41fc11b
