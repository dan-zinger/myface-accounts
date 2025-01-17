﻿import React, {useContext, useEffect, useState} from 'react';
import {fetchUser, User} from "../../Api/apiClient";
import "./UserDetails.scss";
import {LoginContext} from "../../Components/LoginManager/LoginManager";

interface UserDetailsProps {
    userId: string;
}

export function UserDetails(props: UserDetailsProps): JSX.Element {
    const loginContext = useContext(LoginContext);
    const [user, setUser] = useState<User | null>(null);
     
    useEffect(() => {
        fetchUser({logOut: loginContext.logOut, username: loginContext.username, password:loginContext.password},props.userId)
            .then(response => setUser(response));
    }, [props,loginContext.logOut, loginContext.username, loginContext.password]);
    
    if (!user) {
        return <section>Loading...</section>
    }
    
    return (
        <section className="user-details">
            <img className="cover-image" src={user.coverImageUrl} alt="A cover image for the user."/>
            <div className="user-info">
                <img className="profile-image" src={user.profileImageUrl} alt=""/>
                <div className="contact-info">
                    <h1 className="title">{user.displayName}</h1>
                    <div className="username">{user.username}</div>
                    <div className="email">{user.email}</div>
                </div>
            </div>
        </section>
    );
}