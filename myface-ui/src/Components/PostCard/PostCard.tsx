import React, {useContext } from 'react'
import { createInteraction, deletePost, Post } from '../../Api/apiClient'
import { Card } from '../Card/Card'
import './PostCard.scss'
import { Link } from 'react-router-dom'
import { LoginContext } from '../../Components/LoginManager/LoginManager'

interface PostCardProps {
  post: Post
}

export function PostCard(props: PostCardProps): JSX.Element {
  const loginContext = useContext(LoginContext)

  return (
    <Card>
      <div className="post-card">
        <img className="image" src={props.post.imageUrl} alt="" />
        <div className="message">{props.post.message}</div>
        <div>
          <button
            type="submit"
            onClick={() =>
              createInteraction(
                {
                  username: loginContext.username,
                  password: loginContext.password,
                  logOut: loginContext.logOut,
                },
                {  postId: props.post.id, interactionType: 0 },
              )
            }
          >👍:{props.post.likes.length}</button>
          <button
            type="submit"
            onClick={() =>
              createInteraction(
                {
                  username: loginContext.username,
                  password: loginContext.password,
                  logOut: loginContext.logOut,
                },
                { postId: props.post.id, interactionType: 1 },
              )
            }
          >👎:{props.post.dislikes.length}</button>
          {loginContext.role == 1 ? 
            <button
                type="submit"
                onClick={() =>
                deletePost(
                    {
                    username: loginContext.username,
                    password: loginContext.password,
                    logOut: loginContext.logOut,
                    },
                    props.post.id
                )
                }
            >Delete Post</button> : <></> };
        </div>
        <div className="user">
          <img
            className="profile-image"
            src={props.post.postedBy.profileImageUrl}
            alt=""
          />
          <Link className="user-name" to={`/users/${props.post.postedBy.id}`}>
            {props.post.postedBy.displayName}
          </Link>
        </div>
      </div>
    </Card>
  )
}
