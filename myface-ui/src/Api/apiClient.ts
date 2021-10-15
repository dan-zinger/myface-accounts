import internal from "stream"

export interface ListResponse<T> {
  items: T[]
  totalNumberOfItems: number
  page: number
  nextPage: string
  previousPage: string
}

export interface User {
  id: number
  firstName: string
  lastName: string
  displayName: string
  username: string
  email: string
  profileImageUrl: string
  coverImageUrl: string
}

export interface Interaction {
  id: number
  //user: User
  type: string
  //date: string
}

export interface NewInteraction {
  interactionType: number
  postId: number
}

export interface Post {
  id: number
  message: string
  imageUrl: string
  postedAt: string
  postedBy: User
  likes: Interaction[]
  dislikes: Interaction[]
}

export interface NewPost {
  message: string
  imageUrl: string
}

export interface LoginDetails {
  username: string
  password: string
  logOut: () => void
}

export interface LoginResponse {
  role: number
  userId: number
}

export async function fetchUsers(
  loginDetails: LoginDetails,
  searchTerm: string,
  page: number,
  pageSize: number,
): Promise<ListResponse<User>> {
  const response = await fetch(
    `https://localhost:5001/users?search=${searchTerm}&page=${page}&pageSize=${pageSize}`,
    {
      headers: {
        Authorization:
          'Basic ' + btoa(`${loginDetails.username}:${loginDetails.password}`),
      },
    },
  )
  if (response.status === 401) {
    loginDetails.logOut()
  }
  return await response.json()
}

export async function fetchUser(
  loginDetails: LoginDetails,
  userId: string | number,
): Promise<User> {
  const response = await fetch(`https://localhost:5001/users/${userId}`, {
    headers: {
      Authorization:
        'Basic ' + btoa(`${loginDetails.username}:${loginDetails.password}`),
    },
  })
  if (response.status === 401) {
    loginDetails.logOut()
  }
  return await response.json()
}

export async function fetchPosts(
  loginDetails: LoginDetails,
  page: number,
  pageSize: number,
): Promise<ListResponse<Post>> {
  const response = await fetch(
    `https://localhost:5001/feed?page=${page}&pageSize=${pageSize}`,
    {
      headers: {
        Authorization:
          'Basic ' + btoa(`${loginDetails.username}:${loginDetails.password}`),
      },
    },
  )
  if (response.status === 401) {
    loginDetails.logOut()
  }
  return await response.json()
}

export async function fetchPostsForUser(
  loginDetails: LoginDetails,
  page: number,
  pageSize: number,
  userId: string | number,
) {
  const response = await fetch(
    `https://localhost:5001/feed?page=${page}&pageSize=${pageSize}&postedBy=${userId}`,
    {
      headers: {
        Authorization:
          'Basic ' + btoa(`${loginDetails.username}:${loginDetails.password}`),
      },
    },
  )
  if (response.status === 401) {
    loginDetails.logOut()
  }
  return await response.json()
}

export async function fetchPostsLikedBy(
  loginDetails: LoginDetails,
  page: number,
  pageSize: number,
  userId: string | number,
) {
  const response = await fetch(
    `https://localhost:5001/feed?page=${page}&pageSize=${pageSize}&likedBy=${userId}`,
    {
      headers: {
        Authorization:
          'Basic ' + btoa(`${loginDetails.username}:${loginDetails.password}`),
      },
    },
  )
  if (response.status === 401) {
    loginDetails.logOut()
  }
  return await response.json()
}

export async function fetchPostsDislikedBy(
  loginDetails: LoginDetails,
  page: number,
  pageSize: number,
  userId: string | number,
) {
  const response = await fetch(
    `https://localhost:5001/feed?page=${page}&pageSize=${pageSize}&dislikedBy=${userId}`,
    {
      headers: {
        Authorization:
          'Basic ' + btoa(`${loginDetails.username}:${loginDetails.password}`),
      },
    },
  )
  if (response.status === 401) {
    loginDetails.logOut()
  }
  return await response.json()
}

export async function createPost(loginDetails: LoginDetails, newPost: NewPost) {
  const response = await fetch(`https://localhost:5001/posts/create`, {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json',
      Authorization:
        'Basic ' + btoa(`${loginDetails.username}:${loginDetails.password}`),
    },
    body: JSON.stringify(newPost),
  })
  if (response.status === 401) {
    loginDetails.logOut()
  } else if (!response.ok) {
    throw new Error(await response.json())
  }
}

export async function createInteraction(
  loginDetails: LoginDetails,
  interaction: NewInteraction,
) {
  const response = await fetch(`https://localhost:5001/interactions/create`, {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json',
      Authorization:
        'Basic ' + btoa(`${loginDetails.username}:${loginDetails.password}`),
    },
    body: JSON.stringify(interaction),
  })
  if (response.status === 401) {
    loginDetails.logOut()
  } else if (!response.ok) {
    throw new Error(await response.json())
  }
}

export async function deletePost(
  loginDetails: LoginDetails,
  postId: number,
) {
  const response = await fetch(`https://localhost:5001/posts/${postId}`, {
    method: "DELETE",
    headers: {
      Authorization: 'Basic ' + btoa(`${loginDetails.username}:${loginDetails.password}`),
    },
  })
  if (response.status === 401) {
    loginDetails.logOut()
  } else if (response.status === 403) {
    window.alert("Access Denied")
  } else if (!response.ok) {
    throw new Error(await response.json())
  }
}

export async function loginUser(
  loginDetails : LoginDetails,
) : Promise<LoginResponse> {
  const response = await fetch(`https://localhost:5001/login`, {
    headers: {
      Authorization: 'Basic ' + btoa(`${loginDetails.username}:${loginDetails.password}`),
    },
  })
  if (response.status === 401) {
    loginDetails.logOut()
  }  else if (!response.ok) {
    throw new Error(await response.json())
  }
  return await response.json();
}