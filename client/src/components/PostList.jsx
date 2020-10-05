import React, { useContext, useEffect, useState } from "react";
import { PostContext } from "../providers/PostProvider";
import Post from "./Post";

const PostList = () => {
  const { posts, searchTerms, getAllPosts } = useContext(PostContext);
  const [ filteredPosts, setFiltered ] = useState([])

  useEffect(() => {
    getAllPosts();
  }, []);

  useEffect(() => {
    if (searchTerms !== "") {
        const titleSearch = posts.filter(post => post.title.includes(searchTerms))
        setFiltered(titleSearch)
    } else {
        setFiltered(posts)
    }
}, [searchTerms, posts])

  return (
    <div className="container">
      <div className="row justify-content-center">
        <div className="cards-column">
          {filteredPosts.map((post) => (
            <Post key={post.id} post={post} />
          ))}
        </div>
      </div>
    </div>
  );
};

export default PostList;