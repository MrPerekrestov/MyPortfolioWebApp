import React, { Component } from "react";
import BlogCard from "./BlogCard";
class BlogCards extends Component {
    constructor(props) {
        super(props);          
    }  
    render() {
        var blogCards = this.props.blogPosts.map((blogPost, i) => {            
            return <BlogCard {...blogPost}/>
        });
        return <div className="blog-cards-container">{blogCards}</div>;        
    }
}

export default BlogCards;
global.Blogs = BlogCards;




