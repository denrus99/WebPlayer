import React, {useEffect, useState} from "react";
import Song from "./Song";
import '../styles/Album.css';
import PropTypes from "prop-types";
import {getAlbum, getTrack} from "../Fetch";

function Album(props) {
    const [albumState, setAlbumState] = useState({title: "", tracks: [], trackInfos: []});
    useEffect(() => {
        getAlbum(props.match.params.id).then(album => {
            let trackInfos = [];
            Promise.all(album.tracks.map(item => getTrack(item).then(track => trackInfos.push(track))))
                .then(() => {
                setAlbumState(() => {
                    return {...album, trackInfos};
                })
            });
        });
    }, [props.match.params.id]);
    return <div className={"Album"}>
        <h1 className={"AlbumTitle"}>{albumState.photo && <img alt={"Обложка альбома"} src={albumState.photo}/>}{albumState.title}</h1>
        {albumState.trackInfos.map((track, t) => <Song key={track.id} id={t + 1} title={track.title} singer={track.singer}/>)}
    </div>
}

Album.propTypes = {
    match: PropTypes.object
};
export default Album;