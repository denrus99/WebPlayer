import React, {useState} from 'react';
import PropTypes from 'prop-types';
import {autorize, register} from "../Fetch";
import * as Cookies from 'js-cookie';

function FormField(props) {
    return <div className={"Form-field"}>
        <input id={props.id} type={props.fieldType || "text"} onChange={props.changeFunc}/>
        <label htmlFor={props.id}>{props.text}</label>
    </div>
}

FormField.propTypes = {
    id: PropTypes.string,
    text: PropTypes.string,
    fieldType: PropTypes.string,
    changeFunc: PropTypes.func
};

function Registration(props) {
    const [user, setUser] = useState({login: "", password: "", name: "", email: ""});
    const registerFunc = async (e) => {
        e.preventDefault();
        if (user.login !== "" && user.password !== "" && user.name !== "" && user.email !== ""){
            // eslint-disable-next-line no-debugger
            debugger;
            let registerResult = await register(user.login,user.password, user.email, user.name);
            if (registerResult.status){
                Cookies.set("login",registerResult.user.login);
                props.registerFunc();
            }
            else{
                alert("Данные введены неверно.")
            }
        }
        else{
            alert("Для входа необходимо ввести логин и пароль.");
        }
    };
    const loginChange = (e) => {
        let newLogin = e.target.value;
        setUser(prevState => {
            return {...prevState, login: newLogin};
        })
    };
    const passwordChange = (e) => {
        let newPassword = e.target.value;
        setUser(prevState => {
            return {...prevState, password: newPassword}
        })
    };
    const nameChange = (e) => {
        let newName = e.target.value;
        setUser(prevState => {
            return {...prevState, name: newName};
        })
    };
    const emailChange = (e) => {
        let newEmail = e.target.value;
        setUser(prevState => {
            return {...prevState, email: newEmail}
        })
    };
    return <form className={"Registration-form"}>
        <FormField id={"name"} text={"Имя:"} changeFunc={nameChange}/>
        <FormField id={"login"} text={"Логин:"} changeFunc={loginChange}/>
        <FormField id={"password"} text={"Пароль:"} fieldType={"password"} changeFunc={passwordChange}/>
        <FormField id={"email"} text={"E-mail:"} changeFunc={emailChange}/>
        <button onClick={registerFunc}>Отправить</button>
    </form>
}
Registration.propTypes = {
    registerFunc: PropTypes.func.isRequired
};

function Login(props) {
    const [user, setUser] = useState({login: "", password: ""});
    const autorizeFunc = async (e) => {
        e.preventDefault();
        if (user.login !== "" && user.password !== ""){
            let autorizeResult = await autorize(user.login,user.password);
            if (autorizeResult.status){
                Cookies.set("login",autorizeResult.login);
                props.loginFunc();
            }
            else{
                alert("Данные введены неверно.")
            }
        }
        else{
            alert("Для входа необходимо ввести логин и пароль.");
        }
    };
    const loginChange = (e) => {
        let newLogin = e.target.value;
        setUser(prevState => {
            return {...prevState, login: newLogin};
        })
    };
    const passwordChange = (e) => {
        let newPassword = e.target.value;
        setUser(prevState => {
            return {...prevState, password: newPassword}
        })
    };
    return <form className={"Login-form"}>
        <FormField id={"login"} text={"Логин:"} changeFunc={loginChange}/>
        <FormField id={"password"} text={"Пароль:"} fieldType={"password"} changeFunc={passwordChange}/>
        <button onClick={autorizeFunc}>Вход</button>
    </form>
}

Login.propTypes = {
    loginFunc: PropTypes.func.isRequired
};

function Authentication(props) {
    const [login, setLogin] = useState(true);
    const changeTab = () => {
        setLogin(!login)
    };
    return <div className={"Authentication"}>
        <header className="App-header">
            <h1>WebPlayer</h1>
        </header>
        <div className={"Authentication-form"}>
            <div className={"Tabs"}>
                <button onClick={changeTab} disabled={login}
                        className={login ?
                            "Authentication-tab-selected" :
                            "Authentication-tab"}>Вход
                </button>
                <button id={"registration"} onClick={changeTab} disabled={!login}
                        className={login ?
                            "Authentication-tab":
                            "Authentication-tab-selected"}>Регистрация
                </button>
            </div>
            {login ?
                <Login loginFunc={props.changeToMainWindow}/> :
                <Registration registerFunc={props.changeToMainWindow}/>}
        </div>
    </div>
}

Authentication.propTypes = {
    changeToMainWindow: PropTypes.func.isRequired
};

export default Authentication;

