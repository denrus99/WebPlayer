import React from 'react';

function Search() {
    return <div className={"Search"}>
        <img alt={"Иконка поиска"} className={"Search-icon"} src={"/search_icon.png"}/>
        <input className={"Search-field"} placeholder={"Введите данные для поиска..."}/>
    </div>
}

export default Search;