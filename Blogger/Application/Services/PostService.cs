using Application.Dto;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class PostService : IPostService
    {
        private IPostRepository _postRepository;
        private IMapper _mapper;   
        public PostService(IPostRepository postRepository, IMapper mapper)
        {
            _postRepository = postRepository;
            _mapper = mapper;  
        }

        public async Task<IEnumerable<PostDto>> GetAllPostsAsync()
        {
            var posts =  await _postRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<PostDto>>(posts);
        }

        public async Task<PostDto> GetPostByIdAsync(int id)
        {
            var post = await _postRepository.GetByIdAsync(id);
            return _mapper.Map<PostDto>(post);
        }

        public async Task<PostDto> AddNewPostAsync(CreatePostDto newPost)
        {
            if(string.IsNullOrEmpty(newPost.Title))
            {
                throw new Exception("Post can not have an empty title.");
            }
            var post = _mapper.Map<Post>(newPost);
            var result = await _postRepository.AddAsync(post);
            return _mapper.Map<PostDto>(result);
        }

        public async Task UpdatePostAsync(UpdatePostDto updatePost)
        {
            var existingPost = await _postRepository.GetByIdAsync(updatePost.Id);
            var post = _mapper.Map(updatePost, existingPost);
            await _postRepository.UpdateAsync(post);
        }

        public async Task DeletePostAsync(int id)
        {
            var post = await _postRepository.GetByIdAsync(id); 
            await _postRepository.DeleteAsync(post);
        }

        public async Task<IEnumerable<PostDto>> GetPostByTitleAsync(string searchText)
        {
            var posts = await _postRepository.GetByTitleAsync(searchText);
            return _mapper.Map<IEnumerable<PostDto>>(posts);
        }
    }
}
