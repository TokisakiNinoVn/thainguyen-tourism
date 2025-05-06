<template>
    <div class="create-blog">
        <h1>Create a New Blog Post</h1>
        <form @submit.prevent="submitForm">
            <!-- Input Thumbnail -->
            <div class="form-group">
                <label for="thumbnail">Thumbnail:</label>
                <input type="file" id="thumbnail" @change="onThumbnailChange" accept="image/*" required />
                <div v-if="thumbnailPreview" class="preview">
                    <img :src="thumbnailPreview" alt="Thumbnail Preview" class="preview-image" />
                </div>
            </div>

            <!-- Input Title -->
            <div class="form-group">
                <label for="title">Title:</label>
                <input type="text" id="title" v-model="blog.title" required />
            </div>
            
            <!-- Input Title -->
            <div class="form-group">
                <label for="place">Title:</label>
                <input type="text" id="place" v-model="blog.place" required />
            </div>

            <!-- Input Content -->
            <div class="form-group">
                <label class="label">Mô tả</label>
                <quill-editor
                    v-model:content="blog.content"
                    content-type="html"
                    theme="snow"
                    :options="editorOptions"
                    class="editor rounded-lg shadow-sm"
                />
                <p class="mt-2 text-sm text-gray-500">
                    {{ descriptionLength }}/2000 ký tự
                </p>
            </div>

            <!-- Input Media (up to 5 files) -->
            <div class="form-group">
                <label for="media">Media (up to 5 images/videos):</label>
                <input
                    type="file"
                    id="media"
                    @change="onMediaChange"
                    accept="image/*,video/*"
                    multiple
                />
                <div class="previews" v-if="mediaPreviews.length">
                    <div v-for="(preview, index) in mediaPreviews" :key="index" class="preview">
                        <img v-if="preview.type === 'image'" :src="preview.url" alt="Media Preview" class="preview-image" />
                        <video v-else :src="preview.url" controls class="preview-video"></video>
                        <button type="button" @click="removeMedia(index)" class="remove-btn">X</button>
                    </div>
                </div>
                <p v-if="mediaPreviews.length >= 5" class="error">Maximum 5 media files allowed.</p>
            </div>

            <!-- Status -->
            <!-- <div class="form-group">
                <label for="status">Status:</label>
                <select id="status" v-model="blog.status" required>
                    <option value="draft">Draft</option>
                    <option value="published">Published</option>
                </select>
            </div> -->

            <button type="submit" :disabled="isSubmitting">Create Blog</button>
        </form>
    </div>
</template>

<script setup>
import { ref, computed } from 'vue';
import 'quill/dist/quill.snow.css';
import { QuillEditor } from '@vueup/vue-quill';
import { postCreatBlogApi, uploadSingleFileApi } from '@/services/modules/blog.api';
import { useRouter } from 'vue-router';

// Blog data
const blog = ref({
    title: '',
    content: '',
    place: '',
    // status: 'draft',
    // authorId: 1, // Assume a default author ID; replace with actual user ID from auth
    // thumbnail: null,
});

// Thumbnail and media handling
const thumbnailFile = ref(null);
const thumbnailPreview = ref(null);
const mediaFiles = ref([]);
const mediaPreviews = ref([]);
const isSubmitting = ref(false);
const router = useRouter();

// Quill editor options
const editorOptions = {
    placeholder: 'Write your blog content here...',
    modules: {
        toolbar: [
            ['bold', 'italic', 'underline', 'strike'],
            ['blockquote', 'code-block'],
            [{ header: 1 }, { header: 2 }],
            [{ list: 'ordered' }, { list: 'bullet' }],
            [{ indent: '-1' }, { indent: '+1' }],
            ['link', 'image'],
            ['clean'],
        ],
    },
};

// Compute character length for content
const descriptionLength = computed(() => {
    const text = blog.value.content.replace(/<[^>]*>/g, ''); // Strip HTML tags
    return text.length;
});

// Handle thumbnail file change
const onThumbnailChange = (event) => {
    const file = event.target.files[0];
    if (file) {
        thumbnailFile.value = file;
        const reader = new FileReader();
        reader.onload = (e) => {
            thumbnailPreview.value = e.target.result;
        };
        reader.readAsDataURL(file);
    } else {
        thumbnailFile.value = null;
        thumbnailPreview.value = null;
    }
};

// Handle media file change
const onMediaChange = (event) => {
    const files = Array.from(event.target.files);
    if (mediaFiles.value.length + files.length > 5) {
        alert('You can upload a maximum of 5 media files.');
        event.target.value = ''; // Clear input
        return;
    }

    files.forEach((file) => {
        if (file.type.startsWith('image/') || file.type.startsWith('video/')) {
            mediaFiles.value.push(file);
            const reader = new FileReader();
            reader.onload = (e) => {
                mediaPreviews.value.push({
                    url: e.target.result,
                    type: file.type.startsWith('image/') ? 'image' : 'video',
                });
            };
            reader.readAsDataURL(file);
        }
    });
    event.target.value = ''; // Clear input
};

// Remove media file
const removeMedia = (index) => {
    mediaFiles.value.splice(index, 1);
    mediaPreviews.value.splice(index, 1);
};

// Submit form
const submitForm = async () => {
    if (descriptionLength.value > 2000) {
        alert('Content exceeds 2000 characters.');
        return;
    }

    if (!thumbnailFile.value) {
        alert('Please upload a thumbnail.');
        return;
    }

    isSubmitting.value = true;

    try {
        // Create blog
        const blogData = {
            title: blog.value.title,
            content: blog.value.content,
            place: blog.value.place,
            // place: ''
            // status: blog.value.status,
            // authorId: blog.value.authorId,
            // thumbnail: null, // Will be updated after upload
        };

        const response = await postCreatBlogApi(blogData);
        if (response.code === 200 && response.data?.id) {
            const blogId = response.data.id;

            // Upload thumbnail
            const thumbnailFormData = new FormData();
            thumbnailFormData.append('BlogId', blogId);
            thumbnailFormData.append('Type', 1); // 1 for image
            thumbnailFormData.append('ImageFor', 'thumbnail');
            thumbnailFormData.append('File', thumbnailFile.value);

            await uploadSingleFileApi(thumbnailFormData);

            // Upload media files
            for (const file of mediaFiles.value) {
                const mediaFormData = new FormData();
                mediaFormData.append('BlogId', blogId);
                mediaFormData.append('Type', file.type.startsWith('image/') ? 1 : 2); // 1 for image, 2 for video
                mediaFormData.append('ImageFor', 'media');
                mediaFormData.append('File', file);

                await uploadSingleFileApi(mediaFormData);
            }

            alert('Blog created successfully!');
            router.push('/blogs'); // Redirect to blog list or another page
        } else {
            throw new Error('Failed to create blog.');
        }
    } catch (error) {
        console.error('Error creating blog:', error);
        alert('An error occurred while creating the blog.');
    } finally {
        isSubmitting.value = false;
    }
};
</script>

<style scoped>
.create-blog {
    max-width: 800px;
    margin: 0 auto;
    padding: 20px;
}

.form-group {
    margin-bottom: 15px;
}

label {
    display: block;
    margin-bottom: 5px;
    font-weight: bold;
}

input[type="text"],
input[type="file"],
select {
    width: 100%;
    padding: 8px;
    border: 1px solid #ccc;
    border-radius: 4px;
}

.editor {
    min-height: 200px;
}

.previews {
    display: flex;
    flex-wrap: wrap;
    gap: 10px;
    margin-top: 10px;
}

.preview {
    position: relative;
    width: 100px;
    height: 100px;
}

.preview-image,
.preview-video {
    width: 100%;
    height: 100%;
    object-fit: cover;
    border-radius: 4px;
}

.remove-btn {
    position: absolute;
    top: -10px;
    right: -10px;
    background: red;
    color: white;
    border: none;
    border-radius: 50%;
    width: 20px;
    height: 20px;
    cursor: pointer;
}

button[type="submit"] {
    background-color: #28a745;
    color: white;
    padding: 10px 20px;
    border: none;
    border-radius: 4px;
    cursor: pointer;
}

button[type="submit"]:disabled {
    background-color: #ccc;
    cursor: not-allowed;
}

.error {
    color: red;
    font-size: 0.875rem;
}
</style>