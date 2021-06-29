import React, {useState, useEffect} from "react";
import PropTypes from 'prop-types';
import Song from "./Song";
import {getPlaylist, getTrack} from "../Fetch";

function Playlist(props) {
    const [playlistState, setPlaylistState] = useState({title: "", tracks: [], trackInfos: []});
    useEffect(() => {
        getPlaylist(props.match.params.id).then(playlist => {
            let trackInfos = [];
            Promise.all(playlist.tracks.map(item => getTrack(item).then(track => trackInfos.push(track)))).then(() => {
                setPlaylistState(() => {
                    return {...playlist, trackInfos};
                })
            });
        })
    }, [props.match.params.id]);
    return <div className={"Playlist"}>
        <h1 className={"PlaylistTitle"}>{playlistState.title}</h1>
        {playlistState.trackInfos.map((track) => <Song key={track.id} title={track.title} singer={track.singer}/>)}
    </div>
}

Playlist.propTypes = {
    match: PropTypes.object
};

export default Playlist;