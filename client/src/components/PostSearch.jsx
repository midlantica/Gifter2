import React, { useContext } from "react"
import { PostContext } from "../providers/PostProvider"

export const PostSearch = () => {
    const { setTerms } = useContext(PostContext)

    return (
        <>
            Post Search:
            <br />
            <input type="text"
                onKeyUp={
                    (keyEvent) => setTerms(keyEvent.target.value)
                }
                placeholder="Search for a post... " />
        </>
    )
}

export default PostSearch;