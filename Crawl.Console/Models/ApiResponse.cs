namespace Crawl.Console.Models;

using System.Collections.Generic;

public class ApiResponse
{
    public Data Data { get; set; }
    public List<Error> Errors { get; set; }
}

public class Data
{
    public GetReviews GetReviews { get; set; }
}

public class GetReviews
{
    public int TotalCount { get; set; }
    public List<Edge> Edges { get; set; }
    public PageInfo PageInfo { get; set; }
}

public class Edge
{
    public ReviewNode Node { get; set; }
}

public class ReviewNode
{
    public string Typename { get; set; }
    public string Id { get; set; }
    public Creator Creator { get; set; }
    public string RecommendFor { get; set; }
    public object UpdatedAt { get; set; }
    public object CreatedAt { get; set; }
    public bool SpoilerStatus { get; set; }
    public object LastRevisionAt { get; set; }
    public string Text { get; set; }
    public int Rating { get; set; }
    public Shelving Shelving { get; set; }
    public int LikeCount { get; set; }
    public bool? ViewerHasLiked { get; set; }
    public int CommentCount { get; set; }
}

public class Creator
{
    public int Id { get; set; }
    public string ImageUrlSquare { get; set; }
    public bool IsAuthor { get; set; }
    public string ViewerRelationshipStatus { get; set; }
    public int FollowersCount { get; set; }
    public string Typename { get; set; }
    public int TextReviewsCount { get; set; }
    public string Name { get; set; }
    public string WebUrl { get; set; }
    public Contributor Contributor { get; set; }
}

public class Contributor
{
    public string Id { get; set; }
    public ContributorWorksConnection Works { get; set; }
    public string Typename { get; set; }
}

public class ContributorWorksConnection
{
    public int TotalCount { get; set; }
    public string Typename { get; set; }
}

public class Shelving
{
    public Shelf Shelf { get; set; }
    public List<object> Taggings { get; set; }  // Replace with appropriate type if known
    public string WebUrl { get; set; }
    public string Typename { get; set; }
}

public class Shelf
{
    public string Name { get; set; }
    public string WebUrl { get; set; }
    public string Typename { get; set; }
}

public class PageInfo
{
    public string PrevPageToken { get; set; }
    public string NextPageToken { get; set; }
    public string Typename { get; set; }
}

public class Error
{
    public object Path { get; set; }
    public string Message { get; set; }
    public string ErrorType { get; set; }
    public object Data { get; set; }  // Can be replaced with a specific type if known
    public List<ErrorLocation> Locations { get; set; }
}

public class ErrorLocation
{
    public int Line { get; set; }
    public int Column { get; set; }
    public string SourceName { get; set; }
}