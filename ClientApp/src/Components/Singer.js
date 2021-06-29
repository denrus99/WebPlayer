import React, {useEffect, useState} from "react";
import PropTypes from 'prop-types'
import {getAlbum, getPlaylist, getSinger, getTrack} from "../Fetch";
import Song from "./Song";
import '../styles/Singer.css';
import {AlbumPreview} from "./Profile";
import {PlaylistPreview} from "./MusicSelections";

//TODO: убрать заглушки и добавить нормальные значения
function Singer(props) {
    const [singer, setSinger] = useState({
        name: "",
        photo: "",
        tracks: [],
        singers: [],
        playlists: [],
        albums: [],
        trackInfos: [],
        playlistInfos: [],
        albumInfos:[]
    });
    useEffect(() => {
        getSinger(props.match.params.id).then(singer => {
            let trackInfos = [];
            let albumInfos = [];
            let playlistInfos = [];
            Promise.all([
                ...singer.albums.map(item => getAlbum(item).then(album => albumInfos.push(album))),
                ...singer.playlists.map(item => getPlaylist(item).then(playlist => playlistInfos.push(playlist))),
                ...singer.tracks.map(item => getTrack(item).then((track => trackInfos.push(track))))
            ])
                .then(() => {
                    setSinger(() => {
                        return {...singer, trackInfos, albumInfos, playlistInfos}
                    })
                });
        })
    }, [props.match.params.id]);
    return <div className={"SingerComponent"}>
        <div className={"SingerInfo"}>
            <img alt={"Singer photo"} className={"SingerPhoto"} src={singer.photo}/>
            <h2>{singer.name}</h2>
        </div>
        <div className={"SingerMusic"}>
            <h3>Альбомы</h3>
            <div className={"SingerAlbums"}>
                {singer.albumInfos.map(album => <AlbumPreview key={album.id} title={album.title} photo={album.photo}/>)}
            </div>
            <h3>Треки</h3>
            <div className={"SingerTracks"}>
                {singer.trackInfos.map((track, t) => <Song key={track.id} id={t + 1} title={track.title} singer={track.singer}/>)}
            </div>
            <h3>Плейлисты</h3>
            <div className={"SingerPlaylists"}>
                {singer.playlistInfos.map(playlist => <PlaylistPreview key={playlist.id} playlistId={playlist.tracks[0]} playlistTitle={playlist.title}/>)}
            </div>
        </div>
    </div>
}

Singer.propTypes = {
    match: PropTypes.object
};
export default Singer;