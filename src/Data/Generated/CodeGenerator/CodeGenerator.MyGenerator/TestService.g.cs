
using Data.Entities.Example;
namespace CodeGenerator
{
    public class TestService {
        public Guid Id { get; set;}
        private static List<Test> _data = new List<Test>();
        public Test? GetTest(Guid id) => _data.FirstOrDefault(d => d.Id == id);
        public List<Test> GetAll() => _data;
        public void Add(Test test) => _data.Add(test);
        public void Update(Test test) => _data[_data.FindIndex(d => d.Id == test.Id)] = test;
        public void Delete(Guid id) => _data.Remove(_data.FirstOrDefault(d => d.Id == id));
    }
}