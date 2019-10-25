import React, { Component } from "react";
import SearchBar from "./SearchBar";
import BlogCards from "./BlogCards";
import PropTypes from "../../wwwroot/lib/prop-types/prop-types.min.js";

class Blog extends Component {
    constructor(props) {
        super(props);
        this.state = {
            filterText: ""
        };      
        this.changeFilterText = this.changeFilterText.bind(this);
    }

    changeFilterText(filterText) {
        console.log(filterText)
        this.setState((prevState, props) => {
            return {
                filterText: filterText
            }
        });
    }
    
    render() {
        let blogPosts =this.props.blogPosts
            .filter(blogPost => {
                let titleIncludesText = blogPost
                    .title
                    .toUpperCase()
                    .includes(this.state.filterText.toUpperCase());
                let dateRegExp = /[\d]{4}\/[\d]{2}\/[\d]{2}/g;
                let publishedIncludesText = blogPost
                    .published
                    .replace(/-/g, "/")
                    .match(dateRegExp)[0]
                    .includes(this.state.filterText.toUpperCase());
                return titleIncludesText || publishedIncludesText;
            });            
                   
        return (            
            <div className="blog-container" id="my-blog-container">
                <SearchBar changeFilterText={this.changeFilterText}/>
                <BlogCards blogPosts={blogPosts} />
            </div>
        );
    }
}
Blog.defaultProps = {
    blogData :[
        {
            avatarsrc: "/images/blog_avatar.jpg",
            text: "Lorem ipsum dolor, sit amet consectetur adipisicing elit. Voluptate impedit incidunt facilis in! Esse, distinctio voluptatum. Ea cum earum impedit tempore numquam quam quia sequi placeat? Blanditiis ipsam ullam esse.",
            title: "Title one",
            datePublished: "12/12/2019"
        },
        {
            avatarsrc: "/images/blog_avatar.jpg",
            text: "Lorem ipsum dolor, sit amet consectetur adipisicing elit. Voluptate impedit incidunt facilis in! Esse, distinctio voluptatum. Ea cum earum impedit tempore numquam quam quia sequi placeat? Blanditiis ipsam ullam esse.",
            title: "Title two",
            datePublished: "12/12/2019"
        },
        {
            avatarsrc: "/images/c_sharp_logo.png",
            text: "<b>Lorem ipsum dolor</b>,asdasdsad sit amet consectetur adipisicing elit. Voluptate impedit incidunt facilis in! Esse, distinctio voluptatum. Ea cum earum impedit tempore numquam quam quia sequi placeat? Blanditiis ipsam ullam esse. dsadadasdad",
            title: "Title three",
            datePublished:"12/12/2019"
        }
    ]
}

export default Blog;
global.Blog = Blog;




