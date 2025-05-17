import instance from '@/services/axiosConfig';

const getAllBlogApi = async () => instance.get('api/web/blog/list');
const getDetailBlogByIdApi = async (id) => instance.get(`api/web/blog/details/${id}`);
const getCommentBlogByIdApi = async (id) => instance.get(`api/web/blog/comments/${id}`);
const getBlogByUserApi = async () => instance.get(`api/web/blog/user`);

const postCreatBlogApi = async (data) => instance.post('api/web/blog/create', data);
const postCreatCommentBlogApi = async (data) => instance.post('api/web/blog/create-comment', data);

const uploadSingleFileApi = async (formData) => instance.post('api/web/blog/upload-file', 
    formData, {
    headers: {
        'Content-Type': 'multipart/form-data',
    },
});

// delete blog
const deleteBlogApi = async (id) => instance.delete(`api/web/blog/delete/${id}`);

export {    
    getAllBlogApi,
    getDetailBlogByIdApi,
    getCommentBlogByIdApi,
    getBlogByUserApi,
    
    postCreatBlogApi,
    postCreatCommentBlogApi,

    uploadSingleFileApi,

    deleteBlogApi
};