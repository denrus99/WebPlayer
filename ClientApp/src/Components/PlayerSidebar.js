import React, {useEffect, useRef, useState} from 'react';
import PropTypes from 'prop-types'
import 'bootstrap/dist/css/bootstrap.min.css';
import {getAlbumWithTrack, getLastTrack, getUser} from "../Fetch";
import * as Cookies from 'js-cookie';

function AudioButtons(props) {
    return <div className={"AudioButtons"}>
        <button className={"LeftButton"}/>
        <button className={"PlayButton"} onClick={props.togglePlay}/>
        <button className={"RightButton"}/>
        <button className={"AdditionalActions"}/>
    </div>
}

AudioButtons.propTypes = {
    togglePlay: PropTypes.func.isRequired
};

function SongInfo(props) {
    let count = 0;
    return <div className={"SongInfo"}>
        <h2>Информация о треке:</h2>
        <p className={"SongAlbum"}>Альбом: {props.albumTitle}</p>
        <div className={"SongText"}>Текст песни:
            {props.songText.split('\n').map(item => {
                count++;
                return <p key={count}>{item}</p>
            })}</div>
    </div>
}

SongInfo.propTypes = {
    albumTitle: PropTypes.string,
    songText: PropTypes.string
};

const ProgressBar = (props) => {
    const {color, completed, onClickFunc} = props;

    const [down, setDown] = useState(false);

    const mouseDownFunc = () => {
        setDown(true)
    };

    const mouseUpFunc = () => {
        setDown(false)
    };

    const containerStyles = {
        height: 10,
        width: '100%',
        backgroundColor: "#BF7130",
        borderRadius: 50,
        // margin: 50
    };

    const fillerStyles = {
        height: '100%',
        width: `${completed}%`,
        backgroundColor: color,
        borderRadius: 'inherit',
        textAlign: 'right'
    };

    const labelStyles = {
        padding: 5,
        color: 'white',
        fontWeight: 'bold'
    };

    return (
        <div style={containerStyles} onMouseDown={mouseDownFunc} onMouseUp={mouseUpFunc}
             onMouseMove={down ? onClickFunc : null}>
            <div style={fillerStyles}>
                <span style={labelStyles}/>
            </div>
        </div>
    );
};

ProgressBar.propTypes = {
    color: PropTypes.string,
    completed: PropTypes.number,
    onClickFunc: PropTypes.func,
    onMouseHold: PropTypes.func
};

function PlayerSidebar() {
    const [playerState, setPlayerState] = useState({
        progress: 0,
        audio: new Audio(),
        play: false,
        track: {title: "", singer: "", text: ""},
        album: {title: "", photo: ""},
        user: {name: ""}
    });
    const myRef = useRef(null);
    useEffect(() => {
        getUser(Cookies.get("login")).then((response) => {
            setPlayerState(prevState => {
                return {...prevState, user: response}
            });
        });
        getLastTrack("admin").then((response) => {
            setPlayerState(prevState => {
                return {
                    ...prevState,
                    audio: new Audio(`/tracks/${response.fileId}/audio`),
                    track: response
                };
            });
            return response;
        }).then((response) => {
            return getAlbumWithTrack(response.id);
        }).then(response => setPlayerState(prevState => {
            return {...prevState, album: response}
        }));
    }, []);
    const togglePlay = () => {
        setPlayerState(prevState => {
            (prevState.play) ? playerState.audio.pause() : playerState.audio.play();
            return {...prevState, play: !prevState.play}
        });
    };

    function changeProgress(e) {
        setPlayerState(prevState => {
            return {...prevState, progress: e.nativeEvent.offsetX / myRef.current.offsetWidth * 100}
        });
    }

    return <div className={"PlayerSidebar"}>
        <div className={"UserProfile"}>
            <img src={"/avatar.png"} className={"UserImage"} alt={"Аватар пользователя"}/>
            <h2>{playerState.user.name}</h2>
        </div>
        <div className={"SongPreview"}>
            <img alt={"Обложка альбома"} className={"AlbumCover"} src={playerState.album.photo}/>
            <h3 className={"TrackTitle"}>{playerState.track.title}</h3>
            <h4 className={"Singer"}>{playerState.track.singer}</h4>
        </div>
        <div className={"AudioControls"}>
            <div ref={myRef} className={"ProgressBar"}>
                <ProgressBar completed={playerState.progress} color={"#A64B00"} onClickFunc={changeProgress}/>
            </div>
            <AudioButtons togglePlay={togglePlay}/>
        </div>
        <SongInfo albumTitle={playerState.album.title} songText={playerState.track.text}/>
    </div>
}

PlayerSidebar.propTypes = {
    song: PropTypes.shape({
        text: PropTypes.string,
        title: PropTypes.string,
        album: PropTypes.string,
        singer: PropTypes.string
    }),
};

export default PlayerSidebar;