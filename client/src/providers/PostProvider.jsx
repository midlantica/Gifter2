import React, { useState } from "react";

export const PostContext = React.createContext();

export const PostProvider = (props) => {
  const [ posts, setPosts ] = useState([]);
  const [ searchTerms, setTerms ] = useState("")

  const getPost = (id) => {
    return fetch(`/api/post/GetWithPostComments/${id}`).then((res) => res.json());
  };

  const getAllPosts = () => {
    return fetch("/api/post/getwithcomments")
      .then((res) => res.json())
      .then(setPosts);
  };

  const getPostsByUser = (id) => {
    return fetch(`/api/post/users/${id}`)
      .then((res) => res.json())
      .then(setPosts);
  };

  const addPost = (post) => {
    return fetch("/api/post", {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify(post),
    });
  };

  return (
    <PostContext.Provider value={{ 
        searchTerms, posts, getAllPosts, addPost, getPost, setTerms, getPostsByUser
    }}>
      {props.children}
    </PostContext.Provider>
  );
};