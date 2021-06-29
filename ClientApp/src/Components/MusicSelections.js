import React, {useEffect, useState} from 'react';
import PropTypes from 'prop-types';
import {getAlbumWithTrack, getPopularPlaylists} from "../Fetch";
//TODO: исправить получение обложки плейлиста
export function PlaylistPreview(props) {
    const [cover, setCover] = useState("");
    useEffect(() => {
        getAlbumWithTrack(props.playlistId).then((album) => {
            setCover(album.photo)
        })
    }, []);
    return <div className={"PlaylistPreview"}>
        <img src={cover}/>
        <h4 className={"PlaylistPreview"}>{props.playlistTitle}</h4>
    </div>
}

PlaylistPreview.propTypes = {
    playlistTitle: PropTypes.string,
    playlistId: PropTypes.string
};

function MusicSelections() {
    const [state, setState] = useState({selections: []});
    useEffect(() => {
        getPopularPlaylists(6)
            .then(playlists => {
                setState(() => {
                    return {selections: playlists}
                })
            })
    }, []);
    return <div className={"MusicSelections"}>
        <h1>Популярные подборки</h1>
        <div className={"Selections"}>
            {state.selections.map((value) => <PlaylistPreview key={value.id} playlistId={value.tracks[0]}
                                                              playlistTitle={value.title}/>)}
        </div>
    </div>
}

export default MusicSelections;