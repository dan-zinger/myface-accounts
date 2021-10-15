import React, { useContext } from "react";
import { useParams } from 'react-router-dom'; 
import {Page} from "../Page/Page";
import {UserDetails} from "../../Components/UserDetails/UserDetails";
import {PostList} from "../../Components/PostList/PostList";
import {fetchPostsDislikedBy, fetchPostsForUser, fetchPostsLikedBy} from "../../Api/apiClient";
import "./Profile.scss";
import {Users} from "../Users/Users";
import {LoginContext} from "../../Components/LoginManager/LoginManager";

export function Profile(): JSX.Element {
    const {id} = useParams<{id: string}>();
    const loginContext = useContext(LoginContext);
    
    if (id === undefined) {
        // Shouldn't ever happen - but if the ID is somehow undefined, show the base users page.
        return <Users/>
    }
    
    return (
        <Page containerClassName="profile">
            <UserDetails userId={id!}/>
            <div className="activity">
                <PostList title="Posts" fetchPosts={() => fetchPostsForUser({logOut:loginContext.logOut, username: loginContext.username, password:loginContext.password} ,1, 12, id)}/>
                <PostList title="Likes" fetchPosts={() => fetchPostsLikedBy({logOut: loginContext.logOut, username: loginContext.username, password: loginContext.password} ,1, 12, id)}/>
                <PostList title="Dislikes" fetchPosts={() => fetchPostsDislikedBy({logOut: loginContext.logOut, username: loginContext.username, password: loginContext.password },1, 12, id)}/>
            </div>
        </Page>
    );
}