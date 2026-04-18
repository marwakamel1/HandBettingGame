window.hbgLeaderboard = {
  get: function (key) {
    try {
      return localStorage.getItem(key) ?? '[]';
    } catch {
      return '[]';
    }
  },
  set: function (key, value) {
    try {
      localStorage.setItem(key, value);
    } catch {
      /* ignore quota / private mode */
    }
  }
};
