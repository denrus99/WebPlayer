import '../styles/Profile.css'
import '../App.css'
import React, {useEffect, useState} from "react";
import PropTypes from 'prop-types'
import {getUser, getAlbum, getPlaylist, getSinger, getTrack} from "../Fetch";
import Song from "./Song";
import {PlaylistPreview} from "./MusicSelections";

export function SingerPreview(props) {
    return <div className={"SingerPreview"}>
        <img alt={"Фотография исполнителя"} src={props.photo}/>
        <h4 className={"SingerPreview"}>{props.title}</h4>
    </div>
}

SingerPreview.propTypes = {
    photo: PropTypes.string,
    title: PropTypes.string
};

export function AlbumPreview(props) {
    return <div className={"AlbumPreview"}>
        <img alt={"Обложка альбома"} src={props.photo}/>
        <h4 className={"AlbumPreview"}>{props.title}</h4>
    </div>
}

AlbumPreview.propTypes = {
    photo: PropTypes.string,
    title: PropTypes.string
};

function Profile(props) {
    const [user, setUser] = useState({
        name: "",
        photo: "",
        tracks: [],
        singers: [],
        playlists: [],
        albums: [],
        trackInfos: [],
        singerInfos: [],
        playlistInfos: [],
        albumInfos:[]
    });
    useEffect(() => {
        getUser(props.match.params.id).then(user => {
            let trackInfos = [];
            let albumInfos = [];
            let singerInfos = [];
            let playlistInfos = [];
            Promise.all([
                ...user.albums.map(item => getAlbum(item).then(album => albumInfos.push(album))),
                ...user.singers.map(item => getSinger(item).then(singer => singerInfos.push(singer))),
                ...user.playlists.map(item => getPlaylist(item).then(playlist => playlistInfos.push(playlist))),
                ...user.tracks.map(item => getTrack(item).then((track => trackInfos.push(track))))
            ])
                .then(() => {
                    setUser(() => {
                        return {...user, trackInfos, albumInfos, playlistInfos, singerInfos}
                    })
                });
        })
    }, [props.match.params.id]);
    return <div className={"ProfileComponent"}>
        {/*<div className={"ProfileInfo"}>*/}
        {/*    <img alt={"Profile photo"} className={"ProfilePhoto"} src={user.photo || }/>*/}
        {/*    <h2>{user.name}</h2>*/}
        {/*</div>*/}
        <div className={"ProfileMusic"}>
            <h1>Ваша музыка</h1>
            <div className={"ProfileSingers"}>
                <h3>Исполнители</h3>
                {user.singerInfos.map(singer => <SingerPreview key={singer.id} title={singer.name} photo={singer.photo}/>)}
            </div>
            <div className={"ProfileAlbums"}>
                <h3>Альбомы</h3>
                {user.albumInfos.map(album => <AlbumPreview key={album.id} title={album.title} photo={album.photo}/>)}
            </div>
            <div className={"ProfilePlaylists"}>
                <h3>Плейлисты</h3>
                {user.playlistInfos.map(playlist => <PlaylistPreview key={playlist.id} playlistId={playlist.tracks[0]} playlistTitle={playlist.title} photo={playlist.photo}/>)}
            </div>
            <div className={"ProfileTracks"}>
                <h3>Треки</h3>
                {user.trackInfos.map((track, t) => <Song key={track.id} id={t + 1} title={track.title} singer={track.singer}/>)}
            </div>
        </div>
    </div>
}

Profile.propTypes = {
    match: PropTypes.object
};
export default Profile;