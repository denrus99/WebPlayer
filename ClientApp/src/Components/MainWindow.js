import React from 'react';
import PlayerSidebar from "./PlayerSidebar";
import ActionWindow from "./ActionWindow";

function MainWindow() {
    return <div className={"MainWindow"}>
        <PlayerSidebar/>
        <ActionWindow/>
    </div>
}

export default MainWindow;