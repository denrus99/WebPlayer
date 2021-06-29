async function autorize(login, password) {
    let body = {
        login,
        password
    };
    let response = await fetch("/authentication/login", {
        method: "POST",
        headers: {
            'Content-Type': 'application/json;charset=utf-8'
        },
        body: JSON.stringify(body)
    });
    let user = await response.json();
    return {status: response.ok, login: user.login};
}

async function register(login, password, email, name) {
    let body = {
        login,
        password,
        email,
        name
    };
    let response = await fetch("/authentication/register", {
        method: "POST",
        headers: {
            'Content-Type': 'application/json;charset=utf-8'
        },
        body: JSON.stringify(body)
    });
    let user = await response.json();
    return {status: response.ok, user};
}

async function getTrack(trackId) {
    return fetch(`/tracks/${trackId}`)
        .then(response => response.json());
}

async function getPopularPlaylists(count = 10) {
    return fetch(`/playlists/popular/?count=${count}`)
        .then(response => response.json());
}

async function getUser(login) {
    return fetch(`/users/${login}`)
        .then(response => response.json());
}

async function getAlbum(id) {
    return fetch(`/albums/${id}`)
        .then(response => response.json());
}

async function getPlaylist(id) {
    return fetch(`/playlists/${id}`)
        .then(response => response.json());
}

async function getSinger(id) {
    return fetch(`/singers/${id}`)
        .then(response => response.json());
}

async function getLastTrack(id) {
    return fetch(`/users/${id}`)
        .then(userResponse => userResponse.json())
        .then(user => fetch(`/tracks/${user.history[0].trackId}`))
        .then(trackResponse => trackResponse.json());
}

async function getTrackAudio(id) {
    return fetch(`/tracks/${id}/audio`)
        .then(response => response.json());
}

async function getAlbumWithTrack(trackId) {
    return fetch(`/albums?trackId=${trackId}`)
        .then(response => response.json());
}

export {
    autorize,
    register,
    getTrack,
    getPopularPlaylists,
    getUser,
    getAlbum,
    getPlaylist,
    getLastTrack,
    getTrackAudio,
    getSinger,
    getAlbumWithTrack
}