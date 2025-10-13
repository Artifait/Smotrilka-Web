class LinkPageManager {
    constructor() {
        this.init();
    }

    init() {
        this.bindEvents();
    }

    bindEvents() {
        // Добавление/удаление из избранного
        const favoriteBtn = document.getElementById('favoriteBtn');
        if (favoriteBtn) {
            favoriteBtn.addEventListener('click', () => {
                this.toggleFavorite(favoriteBtn);
            });
        }

        // Реакции (лайк/дизлайк)
        document.querySelectorAll('.reaction-btn').forEach(btn => {
            btn.addEventListener('click', (e) => {
                this.handleReaction(e.target);
            });
        });

        // Форма комментария
        const commentForm = document.getElementById('commentForm');
        if (commentForm) {
            commentForm.addEventListener('submit', (e) => {
                this.handleCommentSubmit(e);
            });
        }
    }

    async toggleFavorite(button) {
        const linkId = button.dataset.linkId;
        const isFavorited = button.classList.contains('favorited');

        try {
            let response;
            if (isFavorited) {
                response = await fetch(`/favorites/remove?linkId=${linkId}`, {
                    method: 'DELETE'
                });
            } else {
                response = await fetch(`/favorites/add?linkId=${linkId}`, {
                    method: 'POST'
                });
            }

            if (response.ok) {
                button.classList.toggle('favorited');
                const star = button.querySelector('.star');
                const text = button.querySelector('span:not(.star)');

                if (button.classList.contains('favorited')) {
                    star.textContent = '★';
                    text.textContent = 'В избранном';
                } else {
                    star.textContent = '☆';
                    text.textContent = 'Добавить в избранное';
                }
            } else {
                alert('Ошибка при обновлении избранного');
            }
        } catch (error) {
            console.error('Error toggling favorite:', error);
            alert('Ошибка при обновлении избранного');
        }
    }

    async handleReaction(button) {
        const linkId = button.dataset.linkId;
        const reaction = parseInt(button.dataset.reaction);
        const isActive = button.classList.contains('active');

        // Если кнопка уже активна, снимаем реакцию (reaction = 0)
        const finalReaction = isActive ? 0 : reaction;

        try {
            const response = await fetch('/react', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify({
                    linkId: parseInt(linkId),
                    reaction: finalReaction
                })
            });

            if (response.ok) {
                this.updateReactionUI(button, finalReaction);

                // Обновляем отображение рейтинга (можно доработать для реального обновления)
                const ratingValue = document.querySelector('.rating-value');
                if (ratingValue) {
                    // В реальном приложении здесь нужно получать актуальный рейтинг с сервера
                    const currentRating = parseInt(ratingValue.textContent);
                    let newRating = currentRating;

                    if (isActive) {
                        // Снимаем реакцию
                        newRating += reaction === 1 ? -1 : 1;
                    } else {
                        // Добавляем реакцию
                        newRating += reaction;
                    }

                    ratingValue.textContent = newRating;
                }
            } else {
                alert('Ошибка при отправке реакции');
            }
        } catch (error) {
            console.error('Error sending reaction:', error);
            alert('Ошибка при отправке реакции');
        }
    }

    updateReactionUI(clickedButton, reaction) {
        const allButtons = document.querySelectorAll('.reaction-btn');

        allButtons.forEach(btn => {
            btn.classList.remove('active');
        });

        if (reaction !== 0) {
            clickedButton.classList.add('active');
        }
    }

    handleCommentSubmit(e) {
        const commentText = document.getElementById('commentText');
        const commentError = document.getElementById('commentError');

        if (!commentText.value.trim()) {
            e.preventDefault();
            commentError.textContent = 'Комментарий не может быть пустым';
            return;
        }

        commentError.textContent = '';

        // Показываем индикатор загрузки
        const submitBtn = e.target.querySelector('button[type="submit"]');
        const originalText = submitBtn.textContent;
        submitBtn.textContent = 'Публикация...';
        submitBtn.disabled = true;
    }
}

// Инициализация при загрузке страницы
document.addEventListener('DOMContentLoaded', () => {
    new LinkPageManager();
});