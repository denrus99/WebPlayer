import React from 'react';
import Search from "./Search";
import MusicSelections from "./MusicSelections";
import Profile from "./Profile";
import Album from "./Album";
import Playlist from "./Playlist";
import Singer from "./Singer";
import {
    BrowserRouter as Router,
    Switch,
    Route
} from "react-router-dom";

function ActionWindow() {
    return <div className={"ActionWindow"}>
        <Search/>
        <Router>
            <Switch>
                <Route exact path={"/"} component={MusicSelections}/>
                <Route exact path={"/profile/:id"} component={Profile}/>
                <Route exact path={"/album/:id"} component={Album}/>
                <Route exact path={"/playlist/:id"} component={Playlist}/>
                <Route exact path={"/singer/:id"} component={Singer}/>
            </Switch>
        </Router>
    </div>
}

export default ActionWindow;