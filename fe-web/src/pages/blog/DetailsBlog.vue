<template>
  <div class="main-content">
    <div class="list-image-content">
      <div v-if="isLoading" class="loading">Đang tải...</div>
      <div v-else-if="blogDetails" class="blog-detail">
        <div class="image-viewer">
          <i class="fa-solid fa-chevron-left" @click="changeImage(-1)"></i>
          <img :src="currentImage" class="main-image" alt="Blog Image" />
          <i class="fa-solid fa-chevron-right" @click="changeImage(1)"></i>
        </div>
        <div class="details-blog">
          <div class="post-info">
            <div class="user-info">
              <img
                :src="
                  blogDetails.authorPhoto ||
                  'https://i.pinimg.com/736x/d9/c5/d9/d9c5d921d56bca25b5b8f6f9dd545cb0.jpg'
                "
                class="user-avatar"
                alt="User Avatar"
              />
              <div>
                <span class="user-name">{{
                  blogDetails.authorName || "Bí khách ẩn danh"
                }}</span>
                <br />
                <span class="post-date">{{
                  new Date(blogDetails.createdAt).toLocaleString()
                }}</span>
              </div>
            </div>
            <div class="post-content text-left p-4">
              <span>
                <i class="fa-solid fa-location-dot"></i>
                {{ blogDetails.place }}
              </span>
              <h2 class="font-bold text-xl">{{ blogDetails.title }}</h2>
              <p class="post-description" v-html="blogDetails.content"></p>
              <!-- <p class="reactions">Reactions: {{ blogDetails.reactions || 0 }}</p> -->
            </div>
          </div>
          <div class="comments-section">
            <h3>Bình luận</h3>
            <div v-if="comments.length === 0" class="no-comments">No comments yet.</div>
            <div v-for="(comment, index) in comments" :key="index" class="comment">
              <div class="comment-header">
                <img
                  :src="comment.userAvatar || 'https://via.placeholder.com/30'"
                  class="comment-avatar"
                  alt="Comment Avatar"
                />
                <div>
                  <span class="comment-user">{{ comment.userName || "Anonymous" }}</span>
                  <p class="comment-text">{{ comment.commentContent }}</p>
                  <div class="comment-meta">
                    <span>{{ new Date(comment.createdAt).toLocaleDateString() }}</span>
                  </div>
                </div>
              </div>
            </div>
            <div class="comment-input">
              <textarea
                v-model="newComment.content"
                placeholder="Write your comment..."
                rows="3"
                class="comment-textarea"
              ></textarea>
              <button @clselectick.prevent="submitComment" class="submit-button">
                <i class="fa-solid fa-paper-plane"></i>
              </button>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>
dđ

<script setup>
import { onMounted, ref, computed } from "vue";
import { useRoute } from "vue-router";
import { getDetailBlogByIdApi } from "@/services/modules/blog.api";
import instance from "@/services/axiosConfig";

const route = useRoute();
const blogId = route.params.id;

const blogDetails = ref({});
const mediaList = ref([]);
const comments = ref([]);
const isLoading = ref(true);
const newComment = ref({ rating: 0, content: "" });
const currentImageIndex = ref(0);

const currentImage = computed(() => {
  return mediaList.value.length > 0
    ? `${instance.defaults.baseURL}${
        mediaList.value[currentImageIndex.value]?.mediaUrl ||
        "https://i.pinimg.com/736x/8c/73/9c/8c739c1653be69e5d9224be978c7e425.jpg"
      }`
    : "https://i.pinimg.com/736x/8c/73/9c/8c739c1653be69e5d9224be978c7e425.jpg";
});

const getBlogDetails = async () => {
  try {
    const response = await getDetailBlogByIdApi(blogId);
    const data = response.data.data;
    blogDetails.value = data;
    mediaList.value = data.mediaList || [];
    // Assuming comments are fetched separately or initialized empty
    comments.value = data.comments || [];
  } catch (error) {
    console.error("Error fetching blog details:", error);
  } finally {
    isLoading.value = false;
  }
};

const changeImage = (direction) => {
  const newIndex = currentImageIndex.value + direction;
  if (newIndex >= 0 && newIndex < mediaList.value.length) {
    currentImageIndex.value = newIndex;
  }
};

const submitComment = () => {
  if (newComment.value.content.trim() && newComment.value.rating) {
    const comment = {
      blogId: blogId,
      rating: newComment.value.rating,
      commentContent: newComment.value.content,
      createdAt: new Date().toISOString(),
      userName: "Current User", // Mock user name, replace with actual user data
      userAvatar: "https://via.placeholder.com/30", // Mock avatar
    };
    comments.value.push(comment);
    newComment.value.content = "";
    console.log("Comment submitted:", comment);
    // Note: This is a mock submission. Integrate with an API to save the comment.
  }
};

onMounted(() => {
  getBlogDetails();
});
</script>

<style>
.main-content {
  margin: 0 auto;
  color: black;
  width: 100%;
}

.list-image-content {
  height: 100vh;
}

.loading {
  text-align: center;
  font-size: 18px;
  color: black;
  padding: 20px;
}

.blog-detail {
  display: flex;
  height: 100vh;
}

.image-viewer {
  position: relative;
  width: 70%;
  height: 100%;
  background: #000000;
  display: flex;
  justify-content: center;
  align-items: center;
}

.main-image {
  height: 100%;
  max-height: 100vh;
  max-width: 800px;
  object-fit: cover;
  border-radius: 8px;
}

.fa-chevron-left,
.fa-chevron-right {
  position: absolute;
  top: 50%;
  transform: translateY(-50%);
  font-size: 24px;
  color: #fff;
  background: black;
  padding: 8px;
  border-radius: 50%;
  cursor: pointer;
}

.fa-chevron-left {
  left: 10px;
}

.fa-chevron-right {
  right: 10px;
}

.details-blog {
  flex: 1;
  min-width: 0;
}

.post-info {
  margin-bottom: 20px;
  position: sticky;
  top: 0;
  right: 0;
  background: #dddddd;
  padding: 12px;
}

.user-info {
  display: flex;
  align-items: center;
  margin-bottom: 12px;
}

.user-avatar {
  width: 40px;
  height: 40px;
  border-radius: 50%;
  margin-right: 12px;
}

.user-name {
  font-size: 16px;
  font-weight: 600;
  color: black;
}

.post-date {
  font-size: 12px;
  color: black;
}

.post-content {
  padding: 12px;
  border-radius: 8px;
}

h2 {
  font-size: 20px;
  font-weight: 600;
  color: black;
  margin-bottom: 8px;
}

.post-description {
  font-size: 14px;
  color: rgb(82, 82, 83);
  margin-bottom: 8px;
}

.reactions {
  font-size: 14px;
  color: #b0b3b8;
}

.comments-section {
  margin-top: 16px;
}

.no-comments {
  font-size: 14px;
  color: black;
  padding: 10px;
  border-radius: 8px;
  margin-bottom: 12px;
}

.comment {
  margin-bottom: 12px;
}

.comment-header {
  display: flex;
  align-items: flex-start;
  margin-bottom: 8px;
}

.comment-avatar {
  width: 30px;
  height: 30px;
  border-radius: 50%;
  margin-right: 8px;
}

.comment-user {
  font-size: 14px;
  font-weight: 500;
  color: #e4e6eb;
  margin-bottom: 4px;
}

.comment-text {
  font-size: 14px;
  color: #b0b3b8;
}

.comment-meta {
  display: flex;
  justify-content: space-between;
  font-size: 12px;
  color: #606770;
}

.comment-rating {
  color: #f7b928;
}

.comment-input {
  padding: 12px;
  border-radius: 8px;
  margin-top: 12px;
  position: fixed;
  right: 10px;
  bottom: 10px;
  width: 30vw;
}

h4 {
  font-size: 16px;
  font-weight: 600;
  color: #e4e6eb;
  margin-bottom: 8px;
}

.comment-textarea {
  width: 100%;
  padding: 8px;
  border: 1px solid #3a3b3c;
  border-radius: 4px;
  background: #3a3b3c;
  color: #e4e6eb;
  font-size: 14px;
  margin-bottom: 8px;
  resize: vertical;
}

.rating-select {
  padding: 6px 8px;
  border: 1px solid #3a3b3c;
  border-radius: 4px;
  background: #3a3b3c;
  color: #e4e6eb;
  font-size: 12px;
  margin-bottom: 8px;
}

.submit-button {
  background-color: #2374f2;
  color: white;
  padding: 6px 12px;
  border-radius: 4px;
  font-size: 12px;
  font-weight: 500;
  cursor: pointer;
  transition: background-color 0.3s;
}

.submit-button:hover {
  background-color: #1c5ed6;
}

@media (max-width: 768px) {
  .main-image {
    height: 400px;
  }

  .image-viewer {
    flex: 100%;
  }

  .details-blog {
    flex: 100%;
  }
}
</style>
