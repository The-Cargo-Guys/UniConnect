<script lang="ts" setup>
import { ref, watch } from "vue";
import CommentsDialog from "./CommentsDialog.vue";
import AddCommentsDialog from "./AddCommentsDialog.vue";
import axios from "axios";
import { Post } from "../../apiClient";
import imageUrls from "../../assets/imageUrls";

const props = defineProps(["post"]);
const post: Post = props.post;

var topCommentLiked = ref(false);
var liked = ref(false);
var commentIconEnabled = ref(false);
var commentsDialogActive = ref(false);
var addCommentsDialogActive = ref(false);

var descMaxLength = 200;
var isLongContent = post.content?.length ?? 0 > descMaxLength;
var showFullContent = ref(false);

function formatDate(dateString: string): string {
	if (!dateString) return "";

	const date = new Date(dateString);
	const now = new Date();

	if (isNaN(date.getTime())) return "";

	const diffTime = now.getTime() - date.getTime();
	const diffDays = Math.floor(diffTime / (1000 * 60 * 60 * 24));

	if (diffDays === 0) {
		return "Today";
	} else if (diffDays < 8) {
		return diffDays === 1 ? "1 day ago" : `${diffDays} days ago`;
	} else {
		return `${date.getMonth() + 1}/${date.getDate()}/${date.getFullYear()}`;
	}
}

const formattedTime = ref(formatDate(post.createdAt?.toString() ?? ""));

watch(commentsDialogActive, () => {
	if (!commentsDialogActive.value) {
		commentIconEnabled.value = false;
	}
});

watch(liked, async () => {
	if (liked.value) {
		try {
			await axios.post("api/posts/add-upvote/" + post.id);
		} catch (error) {
			console.log(error);
		}
	} else if (!liked.value) {
		try {
			await axios.post("api/posts/remove-upvote/" + post.id);
		} catch (error) {
			console.log(error);
		}
	}
});

const usedIndices = ref(new Set<number>());
function getRandomImageUrl(): string {
	if (usedIndices.value.size >= imageUrls.length) {
		usedIndices.value.clear();
	}
	let randomIndex: number;
	do {
		randomIndex = Math.floor(Math.random() * imageUrls.length);
	} while (usedIndices.value.has(randomIndex));
	usedIndices.value.add(randomIndex);
	return imageUrls[randomIndex] ?? "";
}
</script>

<template>
	<v-card elevation="12" class="ma-2" rounded="lg">
		<div class="d-flex align-center">
			<!-- Top Bar -->
			<div class="d-flex align-center ma-2">
				<v-avatar class="mr-3" size="32">
					<v-img
						:src="
							post.author?.imagePath?.length == 0
								? '/pp-fallback.png'
								: post.author?.imagePath
						"
						alt="Profile Picture"
					></v-img>
				</v-avatar>
				<p class="font-weight-bold">{{ post.author?.name }}</p>
			</div>
			<v-spacer color="black"></v-spacer>
			<p class="ma-2 text-caption">{{ post.community?.name }}</p>
		</div>
		<div>
			<v-img alt="Post Image" :src="getRandomImageUrl()" />
		</div>
		<div class="d-flex">
			<!-- Likes Bar -->
			<div class="my-2 ml-6">
				<v-icon
					:icon="liked ? 'mdi-heart' : 'mdi-heart-outline'"
					class="mx-1"
					@click="liked = !liked"
					:color="liked ? 'red' : 'black'"
				></v-icon>
				<v-icon
					:icon="commentIconEnabled ? 'mdi-comment' : 'mdi-comment-outline'"
					class="mx-1"
					@click="
						commentsDialogActive = true;
						commentIconEnabled = true;
					"
				></v-icon>
				<v-icon icon="mdi-bookmark-outline" class="mx-1"></v-icon>
			</div>
			<v-spacer class="my-4"></v-spacer>
			<p class="my-2 mr-8">{{ post.upvotes! + (liked ? 1 : 0) }} likes</p>
		</div>
		<v-divider></v-divider>
		<div class="description-wrapper">
			<!-- Username & Description -->
			<p class="text-left ml-3 my-1">
				<strong>{{ post.author?.name }}</strong> •
				<span class="font-weight-thin">{{ formattedTime }}</span>
			</p>
			<v-spacer></v-spacer>
			<div class="mx-3 text-body-2">
				<p v-if="!isLongContent || showFullContent" class="text-left">
					{{ post.content }}
				</p>
				<p v-else class="text-left">
					{{
						post.content?.substring(
							0,
							post.content.lastIndexOf(" ", descMaxLength)
						)
					}}...
				</p>
				<v-btn
					v-if="isLongContent"
					variant="text"
					density="compact"
					class="px-0 text-caption"
					@click="showFullContent = !showFullContent"
				>
					{{ showFullContent ? "Show less" : "Show more" }}
				</v-btn>
			</div>
		</div>
		<v-divider></v-divider>
		<div>
			<!-- Comments -->
			<div>
				<p
					v-if="post.comments && post.comments.length > 0"
					class="text-left ml-3 mb-2 font-weight-light mt-2"
				>
					COMMENTS
				</p>
				<p v-else class="text-left ml-3 mb-2 font-weight-medium mt-2">
					No Comments
				</p>
				<div class="d-flex align-center" v-if="post.comments && post.comments.length > 0">
					<v-avatar class="ml-3" size="28">
						<v-img
							:src="post.comments?.at(0)?.author?.imagePath"
							alt="Profile Picture"
						></v-img>
					</v-avatar>
					<div class="d-flex align-center flex-grow-1 ml-2 mr-3">
						<p class="font-weight-bold text-truncate mr-3 flex-shrink-0">
							{{ post.comments?.at(0)?.author?.name }}
						</p>
						<p class="text-caption text-left flex-grow-1">
							{{ post.comments?.at(0)?.content }}
						</p>
					</div>
				</div>
			</div>
			<div class="mt-1">
				<v-btn
					variant="plain"
					color="grey"
					@click="addCommentsDialogActive = true"
					density="comfortable"
					:ripple="false"
				>
					Add Comment
				</v-btn>
				<v-btn
					v-if="post.comments && post.comments.length > 0"
					variant="plain"
					color="grey"
					@click="commentsDialogActive = true"
					density="comfortable"
					:ripple="false"
				>
					View More Comments
				</v-btn>
			</div>
		</div>
	</v-card>
	<comments-dialog
		v-model:enabled="commentsDialogActive"
		:comments="post.comments"
		:topCommentLiked="topCommentLiked"
	/>
	<add-comments-dialog v-model:enabled="addCommentsDialogActive" />
</template>

<style scoped>
.v-card {
	width: 500px;
	max-height: 1200px;
}

.description-wrapper {
	max-height: 200px;
}
</style>
