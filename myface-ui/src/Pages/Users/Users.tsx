import React, {useState , useContext} from "react";
import {Page} from "../Page/Page";
import {SearchInput} from "../../Components/SearchInput/SearchInput";
import {fetchUsers} from "../../Api/apiClient";
import {UserCard} from "../../Components/UserCard/UserCard";
import {InfiniteList} from "../../Components/InfititeList/InfiniteList";
import "./Users.scss";
import {LoginContext} from "../../Components/LoginManager/LoginManager";
import { LoginDetails } from "../../Api/apiClient";

export function Users(): JSX.Element {
    const loginContext = useContext(LoginContext);
    const [searchTerm, setSearchTerm] = useState("");

    function getUsers(loginDetails: LoginDetails, page: number, pageSize: number) {
        return fetchUsers({logOut: loginContext.logOut , username: loginContext.username , password:loginContext.password},searchTerm, page, pageSize);
    }
    
    return (
        <Page containerClassName="users">
            <h1 className="title">Users</h1>
            <SearchInput searchTerm={searchTerm} updateSearchTerm={setSearchTerm}/>
            <InfiniteList fetchItems={getUsers} renderItem={user => <UserCard key={user.id} user={user}/>}/>
        </Page>
    );
}