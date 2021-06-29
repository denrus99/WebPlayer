import '../styles/Song.css'
import React from "react";
import PropTypes from "prop-types";

function Song(props) {
    return <div className={"Song"}>
        {props.id && <div className={"SongId"}>{props.id}</div>}
        <div className={"SongDescription"}>
            <div className={"SongTitle"}>{props.title}</div>
            <div className={"SongSinger"}>{props.singer}</div>
        </div>
    </div>
}

Song.propTypes = {
    title: PropTypes.string.isRequired,
    singer: PropTypes.string.isRequired,
    id: PropTypes.number
};

export default Song;