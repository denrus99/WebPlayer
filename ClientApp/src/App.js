import './App.css';
import React, {useEffect, useState} from 'react';
import Authentication from "./Components/Authentication";
import MainWindow from "./Components/MainWindow";
import * as Cookies from 'js-cookie'

// class Music extends React.Component {
//   constructor(props) {
//     super(props);
//     this.state = {
//       play: false
//     };
//     this.audio = new Audio(props.url)
//   }
//
//   componentDidMount() {
//     this.audio.addEventListener('ended', () => this.setState({ play: false }));
//   }
//
//   componentWillUnmount() {
//     this.audio.removeEventListener('ended', () => this.setState({ play: false }));
//   }
//
//   togglePlay = () => {
//     this.setState({ play: !this.state.play }, () => {
//       this.state.play ? this.audio.play() : this.audio.pause();
//     });
//   }
//
//   render() {
//     return (
//         <div>
//           <button onClick={this.togglePlay}>{this.state.play ? 'Pause' : 'Play'}</button>
//         </div>
//     );
//   }
// }

function App() {
    const [authenticated, setAuthenticated] = useState(typeof Cookies.get("login") == "string");
    useEffect(() => {
        return () => {
            Cookies.remove("login");
        }
    }, []);
    const changeToMainWindow = () => {
        setAuthenticated(typeof Cookies.get("login") == "string");
    };
    return (
        <div className="App">
            {authenticated ? <MainWindow/> : <Authentication changeToMainWindow={changeToMainWindow}/>}
        </div>
    );
}

export default App;
