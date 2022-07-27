let userId;

const activeUser = document.querySelector("#btnActiveUser")
const deactiveUser = document.querySelector("#btnDeactiveUser")

const setUser = id => userId = id;

const setState = () => {
    const http = window.location.protocol;
    const domain = window.location.host;
    const url = `${http}//${domain}/Admin/SetUserStatus?id=${userId}`;

    window.location = url;
}

activeUser.addEventListener("click", setState);
deactiveUser.addEventListener("click", setState);
